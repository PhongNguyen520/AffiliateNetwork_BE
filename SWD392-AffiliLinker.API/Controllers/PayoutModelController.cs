using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using SWD392_AffiliLinker.Core.Base;
using SWD392_AffiliLinker.Services.DTO.PayoutModelDTO.Request;
using SWD392_AffiliLinker.Services.DTO.PayoutModelDTO.Response;
using SWD392_AffiliLinker.Services.Interfaces;

namespace SWD392_AffiliLinker.API.Controllers
{
	[Route("api/payoutmodel")]
	[ApiController]
	public class PayoutModelController : Controller
	{
		private readonly IPayoutModelService _payoutModelService;
		private readonly ILogger<PayoutModelController> _logger;

		public PayoutModelController(IPayoutModelService payoutModelService, ILogger<PayoutModelController> logger)
		{
			_payoutModelService = payoutModelService;
			_logger = logger;
		}

		[HttpPost("create")]
		public async Task<ActionResult<BaseResponse<string>>> CreatePayoutModel(CreatePayoutModelRequest request)
		{
			try
			{
				await _payoutModelService.CreatePayoutModel(request);
				return Ok(BaseResponse<string>.OkResponse("Sucessful"));
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
				_logger.LogError(ex, "Error when create payout model");
				return StatusCode(500, new BaseResponse<string>
				{
					StatusCode = Core.Store.StatusCodes.ServerError,
					Message = "Có lỗi xảy ra, vui lòng thử lại sau.",
					Code = (int)Core.Store.StatusCodes.ServerError
				});
			}
		}

		[HttpGet]
		public async Task<ActionResult<BaseResponse<IEnumerable<GetPayoutModelsResponse>>>> GetAllPayoutModels()
		{
			try
			{
				var result = await _payoutModelService.GetPayoutModels();
				return Ok(BaseResponse<IEnumerable<GetPayoutModelsResponse>>.OkResponse(result, "Sucessful"));
			}
			catch (BaseException.ErrorException ex)
			{
				_logger.LogError(ex, "Error when create payout model");
				return StatusCode((int)ex.StatusCode, new BaseResponse<string>
				{
					StatusCode = ex.StatusCode,
					Message = ex.ErrorDetail.ErrorMessage.ToString(),
					Code = (int)ex.StatusCode
				});
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error when create payout model");
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
