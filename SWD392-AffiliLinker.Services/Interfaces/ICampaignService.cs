using SWD392_AffiliLinker.Core.Base;
using SWD392_AffiliLinker.Repositories.Entities;
using SWD392_AffiliLinker.Services.DTO.AuthenDTO.Request;
using SWD392_AffiliLinker.Services.DTO.AuthenDTO.Response;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SWD392_AffiliLinker.Services.Interfaces
{
	public interface ICampaignService
	{
		Task<BaseResponse<IEnumerable<CampaignResponse>>> GetAllCampaignsAsync();
		Task<BaseResponse<CampaignResponse>> GetCampaignByIdAsync(string id);
		Task<BaseResponse<CampaignResponse>> CreateCampaignAsync(CampaignRequest request);
		Task<BaseResponse<bool>> UpdateCampaignAsync(string id, CampaignRequest request);
		Task<BaseResponse<bool>> DeleteCampaignAsync(string id);
		Task<BaseResponse<IEnumerable<CampaignResponse>>> FilterCampaignsAsync(string? name, string? status, DateTime? startDate, DateTime? endDate);
		Task<BaseResponse<IEnumerable<CampaignListResponse>>> GetAllCampaignIdsAndNamesAsync();
		Task<BaseResponse<bool>> UpdateCampaignStatusAsync(string id, string status);
	}

}
