using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SWD392_AffiliLinker.Services;
using SWD392_AffiliLinker.Services.DTO.PaymentDTO.Request;
using SWD392_AffiliLinker.Services.Interfaces;
using SWD392_AffiliLinker.Services.Services;

namespace SWD392_AffiliLinker.API.Controllers
{

    [Route("api/vnpay")]
    public class VnPayController : Controller
    {
        private readonly IVnPayService _vpnPayService;
        private readonly ICurrentUserService _currentUserService;
        private readonly ICampaignService _campaignService;
		private readonly IConfiguration _config;
		public VnPayController(IVnPayService vnPayservice, ICurrentUserService currentUserService, ICampaignService campaignService, IConfiguration config)
        {
            _currentUserService = currentUserService;
            _vpnPayService = vnPayservice;
            _campaignService = campaignService;
            _config = config;
        }

        [HttpPost("pay-campaign")]
        public async Task<IActionResult> PayForCampaign([FromBody] PayCampaignRequestModel requestModel)
        {
            try
            {
                // Lấy thông tin campaign
                var campaignResponse = await _campaignService.GetCampaignByIdAsync(requestModel.CampaignId);

                if (campaignResponse.Data == null)
                {
                    return NotFound($"Không tìm thấy chiến dịch với ID {requestModel.CampaignId}");
                }

                // Lấy thông tin user hiện tại
                var userId = _currentUserService.GetUserId();

                // Khởi tạo payload với các giá trị từ campaign
                var payload = new VnPaymentRequestModel
                {
                    Id = campaignResponse.Data.Id,
                    FullName = campaignResponse.Data.CampaignName,
                    Description = $"Thanh toán cho chiến dịch {campaignResponse.Data.CampaignName}",
                    Amount = campaignResponse.Data.Budget, // Use campaign budget for payment amount
                    CreatedDate = DateTime.UtcNow.AddHours(7),
                    UserId = userId,
                    Transaction_Type = "Campaign Payment",
                    Status = "Pending"
                };

                // Tạo URL thanh toán
                var url = _vpnPayService.CreatePaymentUrl(HttpContext, payload);

                // Trả về link thanh toán
                return Ok(url);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Lỗi xử lý thanh toán: {ex.Message}" });
            }
        }


        //[HttpGet("payment-success")]
        //public IActionResult PaymentSuccess()
        //{
        //    return View(); // view để hiển thị thông báo thành công
        //}

        //[HttpGet("payment-failure")]
        //public IActionResult PaymentFailure()
        //{
        //    return View(); // view để hiển thị thông báo thất bại
        //}

        [HttpPost("vnpay")]
        //[Authorize]
        public IActionResult PaymentCalls([FromBody] VnPaymentRequestModel requestModel)
        {


            var userId = _currentUserService.GetUserId();
            
            var payload = new VnPaymentRequestModel
            {
                //OrderId = currentOrderId, 
                FullName = requestModel.FullName,
                Description = requestModel.Description,
                Amount = requestModel.Amount,
                CreatedDate = DateTime.UtcNow.AddHours(7), 
                UserId = userId  

            };


            //  URL thanh toán
            var url = _vpnPayService.CreatePaymentUrl(HttpContext, payload);


            // Trả về link thanh toán
            return Ok(url);
        }




        [HttpGet("api")]
        //[Authorize]
        public async Task<IActionResult> PaymentCallBack()
        {
			string redirectUrl;
			var response = _vpnPayService.PaymentExecute(Request.Query);

            if (response == null || response.VnPayResponseCode != "00")
            {
				//return StatusCode(500, new { message = $"Lỗi thanh toán VNPay: {response?.VnPayResponseCode ?? "unknown error"}" });

				redirectUrl = $"{_config["VnPay:ReturnFailed"]}?status=failed&reason={response?.VnPayResponseCode ?? "unknown"}";
				return Redirect(redirectUrl);
			}

            await _vpnPayService.SaveTransactionAsync(response);

			redirectUrl = $"{_config["VnPay:ReturnSuccess"]}?status=success&transactionId={response.TransactionId}&amount={response.Amount}";
			return Redirect(redirectUrl);
		}


        [HttpGet("transactions")]
        [Authorize]
        public async Task<IActionResult> GetTransactions()
        {
            try
            {
                var userId = _currentUserService.GetUserId();
                var transactions = await _vpnPayService.GetTransactionsAsync(userId);

                return Ok(new
                {
                    message = "Transactions retrieved successfully",
                    transactions = transactions
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error retrieving transactions: {ex.Message}" });
            }
        }

    }
}
