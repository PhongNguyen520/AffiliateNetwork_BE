using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SWD392_AffiliLinker.Repositories.IUOW;
using SWD392_AffiliLinker.Services.DTO.PaymentDTO.Request;
using SWD392_AffiliLinker.Services.DTO.PaymentDTO.Response;
using SWD392_AffiliLinker.Services.Helpers;
using SWD392_AffiliLinker.Services.Interfaces;
using SWD392_AffiliLinker.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure;
using static SWD392_AffiliLinker.Core.Store.EnumStatus;
using Microsoft.EntityFrameworkCore;

namespace SWD392_AffiliLinker.Services.Services
{
    public class VnPayService : IVnPayService
    {
        private readonly IConfiguration _config;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IServiceProvider _serviceProvider;
        private readonly ICurrentUserService _currentUserService;

        // Store payment information during processing
        private static Dictionary<string, (string UserId, decimal Amount, string OrderId)> _paymentTracker
            = new Dictionary<string, (string UserId, decimal Amount, string OrderId)>();

        public VnPayService(IConfiguration config, IUnitOfWork unitOfWork, IServiceProvider serviceProvider, ICurrentUserService currentUserService)
        {
            _config = config;
            _unitOfWork = unitOfWork;
            _serviceProvider = serviceProvider;
            _currentUserService = currentUserService;
            _currentUserService = currentUserService;
        }

        public string CreatePaymentUrl(HttpContext context, VnPaymentRequestModel model)
        {
            var tick = DateTime.Now.Ticks.ToString();
            var vnpay = new VnPayLibrary();

           
            _paymentTracker[tick] = (model.UserId, model.Amount, model.Id);

            vnpay.AddRequestData("vnp_Version", _config["VnPay:Version"]);
            vnpay.AddRequestData("vnp_Command", _config["VnPay:Command"]);
            vnpay.AddRequestData("vnp_TmnCode", _config["VnPay:TmnCode"]);
            vnpay.AddRequestData("vnp_Amount", ((long)(model.Amount * 100)).ToString()); // Convert to integer amount (VNĐ)
            vnpay.AddRequestData("vnp_CreateDate", model.CreatedDate.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", _config["VnPay:CurrCode"]);
            vnpay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress(context));
            vnpay.AddRequestData("vnp_Locale", _config["VnPay:Locale"]);
            vnpay.AddRequestData("vnp_OrderInfo", "Thanh toán cho đơn hàng:" + model.Id);

            vnpay.AddRequestData("vnp_OrderType", "other"); // default value: other
            var callback = _config["VnPay:PaymentBackReturnUrl"];
            vnpay.AddRequestData("vnp_ReturnUrl", callback);
            vnpay.AddRequestData("vnp_TxnRef", tick); // Transaction reference ID, must be unique per day

            // Create the payment URL
            var paymentUrl = vnpay.CreateRequestUrl(_config["VnPay:BaseUrl"], _config["VnPay:HashSecret"]);

            return paymentUrl;
        }

        public VNPaymentResponseModel PaymentExecute(IQueryCollection collections)
        {
            var vnpay = new VnPayLibrary();
            foreach (var (key, value) in collections)
            {
                if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
                {
                    vnpay.AddResponseData(key, value.ToString());
                }
            }

            var responseTicket = vnpay.GetResponseData("vnp_TxnRef");
            var (userId, amount, campaignId) = _paymentTracker.GetValueOrDefault(responseTicket);
            _paymentTracker.Remove(responseTicket);

            var vnp_orderId = Convert.ToInt64(responseTicket);
            var vnp_TransactionId = Convert.ToInt64(vnpay.GetResponseData("vnp_TransactionNo"));
            var vnp_SecureHash = collections.FirstOrDefault(p => p.Key == "vnp_SecureHash").Value;
            var vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
            var vnp_OrderInfo = vnpay.GetResponseData("vnp_OrderInfo");

            bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, _config["VnPay:HashSecret"]);
            if (!checkSignature)
            {
                return new VNPaymentResponseModel
                {
                    Success = false
                };
            }

            var response = new VNPaymentResponseModel
            {
                Success = true,
                PaymentMethod = "VnPay",
                OrderDescription = vnp_OrderInfo,
                Id = campaignId, 
                TransactionId = vnp_TransactionId.ToString(),
                Token = vnp_SecureHash,
                VnPayResponseCode = vnp_ResponseCode,
                UserId = userId,
                Amount = amount
            };

            if (response.Success && vnp_ResponseCode == "00")
            {
                try
                {
                    var campaignRepo = _unitOfWork.GetRepository<Campaign>();
                    var campaign = campaignRepo.Entities.FirstOrDefault(c => c.Id == campaignId);

                    if (campaign != null)
                    {
                        campaign.Status = CampaignStatus.Active.ToString(); // Cập nhật trạng thái thành Active
                        campaign.LastUpdatedTime = DateTime.UtcNow;

                        campaignRepo.Update(campaign);
                        _unitOfWork.SaveAsync().Wait();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Lỗi khi cập nhật trạng thái campaign: " + ex.Message);
                }
            }

            return response;
        }
        public async Task SaveTransactionAsync(VNPaymentResponseModel response)
        {
            _unitOfWork.BeginTransaction();
            try
            {
                // Fetch the campaign to get its name
                var campaignRepo = _unitOfWork.GetRepository<Campaign>();
                var campaign = campaignRepo.Entities.FirstOrDefault(c => c.Id == response.Id);

                var transaction = new Transaction
                {
                    Amount = response.Amount,
                    Transaction_Type = "VnPay Payment",
                    Description = campaign != null
                        ? $"Đã thanh toán thành công cho chiến dịch {campaign.CampaignName}"
                        : response.OrderDescription,
                    Status = response.Success ? "Success" : "Failed",
                    UserId = Guid.Parse(response.UserId),
                    CreatedTime = DateTimeOffset.UtcNow,
                    LastUpdatedTime = DateTimeOffset.UtcNow
                };

                await _unitOfWork.GetRepository<Transaction>().InsertAsync(transaction);
                await _unitOfWork.SaveAsync();
                _unitOfWork.CommitTransaction();
            }
            catch (Exception ex)
            {
                _unitOfWork.RollBack();
                await Console.Out.WriteLineAsync(ex.Message);
            }
        }


        public async Task<IEnumerable<TransactionResponseModel>> GetTransactionsAsync(string userId)
        {
            try
            {
                var transactionRepo = _unitOfWork.GetRepository<Transaction>();

                var transactions = await transactionRepo.Entities
                    .Where(t => t.UserId == Guid.Parse(userId))
                    .OrderByDescending(t => t.CreatedTime)
                    .Select(t => new TransactionResponseModel
                    {
                        Id = t.Id,
                        Amount = t.Amount,
                        Transaction_Type = t.Transaction_Type,
                        Description = t.Description,
                        Status = t.Status,
                        UserId = t.UserId.ToString(),
                        CreatedTime = t.CreatedTime
                    })
                    .ToListAsync();

                return transactions;
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error retrieving transactions: {ex.Message}");
                return new List<TransactionResponseModel>();
            }
        }
    }
}