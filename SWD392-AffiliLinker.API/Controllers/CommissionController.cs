using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWD392_AffiliLinker.Core.Base;
using SWD392_AffiliLinker.Services.DTO.CommissionDTO.Request;
using SWD392_AffiliLinker.Services.DTO.CommissionDTO.Response;
using SWD392_AffiliLinker.Services.Interfaces;

namespace SWD392_AffiliLinker.API.Controllers
{
    [Route("api/commission")]
    [ApiController]
    public class CommissionController : ControllerBase
    {
        private readonly ICommissionService _commissionService;
        public CommissionController(ICommissionService commissionService)
        {
            _commissionService = commissionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetListCommission([FromQuery]FillterCommission request)
        {
            try
            {
                var result = await _commissionService.GetListCommission(request);
                return Ok(BaseResponse<List<CommissionResponse>>.OkResponse(result, "Successfull"));
            }
            catch (BaseException.ErrorException ex)
            {
                return StatusCode((int)ex.StatusCode, new BaseResponse<string>
                {
                    StatusCode = ex.StatusCode,
                    Message = ex.ErrorDetail.ErrorMessage.ToString(),
                    Code = (int)ex.StatusCode
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseResponse<string>
                {
                    StatusCode = Core.Store.StatusCodes.ServerError,
                    Message = "Có lỗi xảy ra, vui lòng thử lại sau.",
                    Code = (int)Core.Store.StatusCodes.ServerError
                });
            }
        }
    }
}
