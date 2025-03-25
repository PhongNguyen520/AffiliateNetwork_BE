using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SWD392_AffiliLinker.Core.Base;
using SWD392_AffiliLinker.Services.DTO.CampaginDTO.Request;
using SWD392_AffiliLinker.Services.DTO.CampaginDTO.Response;
using SWD392_AffiliLinker.Services.Interfaces;
using System.Threading.Tasks;
using static SWD392_AffiliLinker.Core.Store.EnumStatus;

namespace SWD392_AffiliLinker.API.Controllers
{
    [Route("api/campaign")]
	[ApiController]
	public class CampaignController : ControllerBase
	{
		private readonly ICampaignService _campaignService;

		public CampaignController(ICampaignService campaignService)
		{
			_campaignService = campaignService;
		}

		/// <summary>
		/// Publisher lấy danh sách chiến dịch đã join để tạo link.
		/// </summary>
		/// <returns>CampaignId and CampaignName</returns>
		[HttpGet("list_join_campaign")]
		[Authorize(Roles = "Publisher")]
		public async Task<IActionResult> GetCampaignList()
		{
			try
			{
				var response = await _campaignService.GetAllCampaignIdsAndNamesAsync();
				return Ok(response);
			}
			catch (System.Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex.Message}");
			}
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetCampaignById(string id)
		{
			try
			{
				if (string.IsNullOrEmpty(id))
				{
					return BadRequest("ID cannot be null or empty");
				}

				var response = await _campaignService.GetCampaignByIdAsync(id);
				if (response.Data == null)
				{
					return NotFound($"Campaign with ID {id} not found");
				}

				return Ok(response);
			}
			catch (System.Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex.Message}");
			}
		}

		[HttpPost("create")]
		[Authorize(Roles = "Advertiser")]
		public async Task<IActionResult> CreateCampaign(CreateCampaignRequest request)
		{
			try
			{
				if (request == null)
				{
					return BadRequest("Campaign request cannot be null");
				}

				// Basic validation
				if (string.IsNullOrEmpty(request.CampaignName))
				{
					return BadRequest("Campaign name is required");
				}

				if (string.IsNullOrEmpty(request.WebsiteLink))
				{
					return BadRequest("Website link is required");
				}

				if (request.ConversionRate == null)
				{
					return BadRequest("ConversionRate is required");
				}

				var response = await _campaignService.CreateCampaignAsync(request);

				if (response.Data == null)
				{
					return BadRequest(response.Message ?? "Failed to create campaign");
				}

				return CreatedAtAction(nameof(GetCampaignById), new { id = response.Data.Id }, response);
			}
			catch (System.Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex.Message}");
			}
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateCampaign(string id, [FromBody] CampaignRequest request)
		{
			try
			{
				if (string.IsNullOrEmpty(id))
				{
					return BadRequest("ID cannot be null or empty");
				}

				if (request == null)
				{
					return BadRequest("Campaign request cannot be null");
				}

				var response = await _campaignService.UpdateCampaignAsync(id, request);

				// For bool responses, check if successful
				if (response.Data == false)
				{
					return NotFound(response.Message ?? $"Campaign with ID {id} not found or update failed");
				}

				return NoContent();
			}
			catch (System.Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex.Message}");
			}
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteCampaign(string id)
		{
			try
			{
				if (string.IsNullOrEmpty(id))
				{
					return BadRequest("ID cannot be null or empty");
				}

				var response = await _campaignService.DeleteCampaignAsync(id);

				// For bool responses, check if successful
				if (response.Data == false)
				{
					return NotFound(response.Message ?? $"Campaign with ID {id} not found or deletion failed");
				}

				return NoContent();
			}
			catch (System.Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex.Message}");
			}
		}

		/// <summary>
		/// Admin get list campaign chưa duyệt.
		/// </summary>
		[HttpGet("get_wait_list")]
		public async Task<ActionResult<BaseResponse<BasePaginatedList<CampaignResponse>>>> GetWaitList(
	[FromQuery] int pageIndex,
	[FromQuery] int pageSize)
		{
			try
			{
				var response = await _campaignService.GetWaitCampaignList(pageIndex, pageSize);
				return Ok(BaseResponse<BasePaginatedList<CampaignResponse>>.OkResponse(response, "Sucessful"));
			}
			catch (System.Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex.Message}");
			}
		}


		[HttpGet("filter")]
		public async Task<ActionResult<BaseResponse<BasePaginatedList<CampaignFilterResponse>>>> FilterCampaigns(
	[FromQuery] string? name,
	[FromQuery] string? status,
	[FromQuery] DateTime? startDate,
	[FromQuery] DateTime? endDate,
	[FromQuery] string? payoutMethodId,
	[FromQuery] string? categoryId,
	[FromQuery] int pageIndex,
	[FromQuery] int pageSize)
		{
			try
			{
				var response = await _campaignService.FilterCampaignsAsync(name, status, startDate, endDate, payoutMethodId, categoryId, pageIndex, pageSize);
				return Ok(BaseResponse<BasePaginatedList<CampaignFilterResponse>>.OkResponse(response, "Sucessful"));
			}
			catch (System.Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex.Message}");
			}
		}

		[HttpPatch("status")]
		public async Task<IActionResult> UpdateStatus(string id, CampaignStatus request)
		{
			try
			{
				if (string.IsNullOrEmpty(id))
				{
					return BadRequest("ID cannot be null or empty");
				}

				var response = await _campaignService.UpdateCampaignStatusAsync(id, request);

				if (response.Data == false)
				{
					return NotFound(response.Message ?? $"Campaign with ID {id} not found or status update failed");
				}

				return Ok(response);
			}
			catch (System.Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex.Message}");
			}
		}

		[HttpGet("advertiser_list")]
		public async Task<ActionResult<BaseResponse<BasePaginatedList<CampaignFilterResponse>>>> AdvertisersCampaigns(
	[FromQuery] string? name,
	[FromQuery] string? status,
	[FromQuery] int pageIndex,
	[FromQuery] int pageSize)
		{
			try
			{
				var response = await _campaignService.GetAdvertisorCampaignsAsync(name, status, pageIndex, pageSize);
				return Ok(BaseResponse<BasePaginatedList<CampaignFilterResponse>>.OkResponse(response, "Sucessful"));
			}
			catch (System.Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex.Message}");
			}
		}
	}
}