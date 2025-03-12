using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SWD392_AffiliLinker.Core.Base;
using SWD392_AffiliLinker.Services.Interfaces;

namespace SWD392_AffiliLinker.API.Controllers
{
	[Route("api/campaignmember")]
	[ApiController]
	public class CampaignMemberController : Controller
	{
		private readonly ICampaignMemberService _campaignMemberService;
		private readonly ILogger<CampaignMemberController> _logger;

		public CampaignMemberController(ICampaignMemberService campaignMemberService, ILogger<CampaignMemberController> logger)
		{
			_campaignMemberService = campaignMemberService;
			_logger = logger;
		}

		/// <summary>
		/// Publisher join campaign
		/// </summary>
		[HttpPost("{id}/join")]
		[Authorize(Roles = "Publisher")]
		public async Task<ActionResult<BaseResponse<string>>> JoinCampaign(string id)
		{
			try
			{
				await _campaignMemberService.JoinCampaign(id);
				return Ok(BaseResponse<string>.OkResponse("Successful"));
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
				_logger.LogError(ex, "Error when join campaign");
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
