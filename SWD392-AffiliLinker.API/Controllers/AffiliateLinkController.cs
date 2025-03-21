using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SWD392_AffiliLinker.Core.Base;
using SWD392_AffiliLinker.Repositories.Entities;
using SWD392_AffiliLinker.Services.DTO.AffiliateLinkDTO.Request;
using SWD392_AffiliLinker.Services.DTO.AffiliateLinkDTO.Response;
using SWD392_AffiliLinker.Services.Interfaces;

namespace SWD392_AffiliLinker.API.Controllers
{
	[Route("api/affiliatelink")]
	[ApiController]
	public class AffiliateLinkController : Controller
	{
		private readonly IAffiliateLinkService _affiliateLinkService;
		private readonly ILogger<AffiliateLinkController> _logger;

		public AffiliateLinkController(IAffiliateLinkService affiliateLinkService, ILogger<AffiliateLinkController> logger)
		{
			_affiliateLinkService = affiliateLinkService;
			_logger = logger;
		}

		/// <summary>
		/// Danh sách links của Publisher khi vào trang manage links.
		/// </summary>
		[HttpGet]
		[Authorize(Roles = "Publisher")]
		public async Task<ActionResult<BaseResponse<BasePaginatedList<GetLinksResponse>>>> GetLinks(int pageIndex, int pageSize)
		{
			try
			{
				var result = await _affiliateLinkService.GetPublisherLinkList(pageIndex, pageSize);
				return Ok(BaseResponse<BasePaginatedList<GetLinksResponse>>.OkResponse(result, "Successful"));
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
				_logger.LogError(ex, "Error when get affiliate link.");
				return StatusCode(500, new BaseResponse<string>
				{
					StatusCode = Core.Store.StatusCodes.ServerError,
					Message = "Có lỗi xảy ra, vui lòng thử lại sau.",
					Code = (int)Core.Store.StatusCodes.ServerError
				});
			}
		}

		/// <summary>
		/// Danh sách links của 1 Campaign trong List Joined Campaign.
		/// </summary>
		[HttpGet("getlinks")]
		[Authorize(Roles = "Publisher")]
		public async Task<ActionResult<BaseResponse<GetLinksResponse>>> GetLinksByCampaignId([FromQuery] FilterLinkRequest request)
		{
			try
			{
				var result = await _affiliateLinkService.GetLinkListByCampaignId(request);
				return Ok(BaseResponse<BasePaginatedList<GetLinksResponse>>.OkResponse(result, "Successful"));
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
				_logger.LogError(ex, "Error when get affiliate link.");
				return StatusCode(500, new BaseResponse<string>
				{
					StatusCode = Core.Store.StatusCodes.ServerError,
					Message = "Có lỗi xảy ra, vui lòng thử lại sau.",
					Code = (int)Core.Store.StatusCodes.ServerError
				});
			}
		}

		[HttpPost("createlink")]
		[Authorize(Roles = "Publisher")]
		public async Task<ActionResult<BaseResponse<CreateLinkResponse>>> CreateLink(CreateLinkRequest request)
		{
			try
			{
				var result = await _affiliateLinkService.CreateLink(request);
				return Ok(BaseResponse<CreateLinkResponse>.OkResponse(result, "Successfull"));

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
				_logger.LogError(ex, "Error when create affiliate link.");
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
