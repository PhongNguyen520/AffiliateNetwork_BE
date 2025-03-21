using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        
        public VnPayController(IVnPayService vnPayservice, ICurrentUserService currentUserService)
        {
           _currentUserService = currentUserService;
            _vpnPayService = vnPayservice;
        }
        [HttpPost("vnpay")]
        //[Authorize]
        public IActionResult PaymentCalls([FromBody] VnPaymentRequestModel requestModel)
        {


            var userId = _currentUserService.GetUserId();
            // Khởi tạo payload với các giá trị từ requestModel
            var payload = new VnPaymentRequestModel
            {
                //OrderId = currentOrderId, 
                FullName = requestModel.FullName,
                Description = requestModel.Description,
                Amount = requestModel.Amount,
                CreatedDate = DateTime.UtcNow.AddHours(7), // Đặt thời gian hiện tại (UTC+7)
                UserId = userId  // Include UserId in payload

            };


            // Tạo URL thanh toán
            var url = _vpnPayService.CreatePaymentUrl(HttpContext, payload);


            // Trả về link thanh toán
            return Ok(url);
        }




        [HttpGet("api")]
        //[Authorize]
        public async Task<IActionResult> PaymentCallBack()
        {

            var response = _vpnPayService.PaymentExecute(Request.Query);


            if (response == null || response.VnPayResponseCode != "00")
            {
                return StatusCode(500, new { message = $"Lỗi thanh toán VNPay: {response?.VnPayResponseCode ?? "unknown error"}" });
            }
          await _vpnPayService.SaveTransactionAsync(response);

            return Ok(new
            {
                message = "Thanh toán thành công!",
                transaction = new
                {
                    PaymentMethod = response.PaymentMethod,
                    OrderDescription = response.OrderDescription,
                    Id = response.Id,
                    TransactionId = response.TransactionId,
                    Amount = response.Amount,
                    Status = response.Success ? "Success" : "Failed",
                    CreatedDate = DateTime.UtcNow.AddHours(7)
                }
            });


        }
        

    }
}
