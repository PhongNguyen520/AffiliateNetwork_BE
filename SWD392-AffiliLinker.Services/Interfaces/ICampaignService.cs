using Microsoft.AspNetCore.Http;
using SWD392_AffiliLinker.Core.Base;
using SWD392_AffiliLinker.Repositories.Entities;
using SWD392_AffiliLinker.Services.DTO.CampaginDTO.Request;
using SWD392_AffiliLinker.Services.DTO.CampaginDTO.Response;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static SWD392_AffiliLinker.Core.Store.EnumStatus;

namespace SWD392_AffiliLinker.Services.Interfaces
{
    public interface ICampaignService
	{
		Task<BaseResponse<CampaignDetailResponse>> GetCampaignByIdAsync(string id);
		Task<BaseResponse<CampaignResponse>> CreateCampaignAsync(CreateCampaignRequest request);
		Task<BaseResponse<bool>> UpdateCampaignAsync(string id, CampaignRequest request);
		Task<BaseResponse<bool>> DeleteCampaignAsync(string id);
		Task<BasePaginatedList<CampaignFilterResponse>> FilterCampaignsAsync(string? name, string? status, DateTime? startDate, DateTime? endDate, string? payoutMethod, string? category, int? index, int? size);
		Task<BaseResponse<IEnumerable<CampaignListResponse>>> GetAllCampaignIdsAndNamesAsync();
		Task<BaseResponse<bool>> UpdateCampaignStatusAsync(string id, CampaignStatus status);
		Task<BasePaginatedList<CampaignResponse>> GetWaitCampaignList(int? index, int? size);
		Task<BasePaginatedList<CampaignFilterResponse>> GetAdvertisorCampaignsAsync(string? name, string? status, int? index, int? size);
	}

}
