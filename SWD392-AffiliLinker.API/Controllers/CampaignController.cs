using Microsoft.AspNetCore.Mvc;
using SWD392_AffiliLinker.Core.Base;
using SWD392_AffiliLinker.Services.DTO.AuthenDTO.Request;
using SWD392_AffiliLinker.Services.Interfaces;
using System.Threading.Tasks;

namespace SWD392_AffiliLinker.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CampaignController : ControllerBase
	{
		private readonly ICampaignService _campaignService;

		public CampaignController(ICampaignService campaignService)
		{
			_campaignService = campaignService;
		}

		[HttpGet("GetAll")]
		public async Task<IActionResult> GetAllCampaigns()
		{
			try
			{
				var response = await _campaignService.GetAllCampaignsAsync();
				return Ok(response);
			}
			catch (System.Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex.Message}");
			}
		}

		[HttpGet("list_campaignid_name")]
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

		[HttpPost("Create")]
		public async Task<IActionResult> CreateCampaign([FromBody] CampaignRequest request)
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

		[HttpGet("filter")]
		public async Task<IActionResult> FilterCampaigns(
	[FromQuery] string? name,
	[FromQuery] string? status,
	[FromQuery] DateTime? startDate,
	[FromQuery] DateTime? endDate)
		{
			try
			{
				var response = await _campaignService.FilterCampaignsAsync(name, status, startDate, endDate);
				return Ok(response);
			}
			catch (System.Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex.Message}");
			}
		}

		[HttpPatch("Status")]
		public async Task<IActionResult> UpdateStatus(string id, [FromBody] UpdateStatusRequest request)
		{
			try
			{
				if (string.IsNullOrEmpty(id))
				{
					return BadRequest("ID cannot be null or empty");
				}

				if (request == null || string.IsNullOrEmpty(request.Status))
				{
					return BadRequest("Status cannot be null or empty");
				}

				// Validate status value
				if (request.Status != "1" && request.Status != "-1")
				{
					return BadRequest("Status must be either '1' (approved) or '-1' (rejected)");
				}

				var response = await _campaignService.UpdateCampaignStatusAsync(id, request.Status);

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
	}
}