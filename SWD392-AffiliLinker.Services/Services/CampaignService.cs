using Microsoft.EntityFrameworkCore;
using SWD392_AffiliLinker.Repositories.Entities;
using SWD392_AffiliLinker.Repositories.IUOW;
using SWD392_AffiliLinker.Core.Base;
using SWD392_AffiliLinker.Services.Interfaces;
using SWD392_AffiliLinker.Services.DTO.CampaginDTO.Request;
using SWD392_AffiliLinker.Services.DTO.CampaginDTO.Response;
using AutoMapper;
using static SWD392_AffiliLinker.Core.Store.EnumStatus;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SWD392_AffiliLinker.Services
{
    public class CampaignService : ICampaignService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly ICurrentUserService _currentUserService;
		private readonly IMapper _mapper;
        private readonly IHepperUploadImage _hepperUploadImage;

        public CampaignService(IUnitOfWork unitOfWork, ICurrentUserService currentUserService, IMapper mapper, IHepperUploadImage hepperUploadImage)
		{
			_unitOfWork = unitOfWork;
			_currentUserService = currentUserService;
			_mapper = mapper;
			_hepperUploadImage = hepperUploadImage;
		}

		public async Task<BaseResponse<CampaignResponse>> CreateCampaignAsync([FromBody]CreateCampaignRequest campaignRequest)
		{
			_unitOfWork.BeginTransaction();
			try
			{

                using var stream = campaignRequest.Image.OpenReadStream();
                var url = await _hepperUploadImage.UploadImageAsync(stream, campaignRequest.Image.FileName);

                var userId = _currentUserService.GetUserId();
				var campaign = _mapper.Map<Campaign>(campaignRequest);
				campaign.UserId = Guid.Parse(userId);
				campaign.EnrollCount = 0;
				campaign.CreatedTime = DateTime.Now;
				campaign.LastUpdatedTime = DateTime.Now;
				campaign.Status = CampaignStatus.Wait.ToString();
				campaign.ConversionRate = campaignRequest.ConversionRate;
				campaign.Image = url;

				if (string.IsNullOrEmpty(campaignRequest.CategoryId))
				{
					campaign.CategoryId = null;
				}
				await _unitOfWork.GetRepository<Campaign>().InsertAsync(campaign);
				var payoutModels = campaignRequest.PayoutModelsId.Select(payoutId => new CampaignPayoutModel
				{
					PayoutModelId = payoutId,
					CampaignId = campaign.Id,
					CreatedTime = DateTime.UtcNow,
					LastUpdatedTime = DateTime.UtcNow,
					Status = MemberStatus.Active.ToString()
				}).ToList();

				_unitOfWork.GetRepository<CampaignPayoutModel>().InsertRange(payoutModels);
				await _unitOfWork.SaveAsync();
				_unitOfWork.CommitTransaction();

				var response = _mapper.Map<CampaignResponse>(campaign);

				return BaseResponse<CampaignResponse>.OkResponse(response, "Campaign created successfully");
			}
			catch (Exception ex)
			{
				_unitOfWork.RollBack();
				return BaseResponse<CampaignResponse>.FailResponse($"Error: {ex.Message} | Inner: {ex.InnerException?.Message}");
			}
		}

		public async Task<BaseResponse<IEnumerable<CampaignListResponse>>> GetAllCampaignIdsAndNamesAsync()
		{
			try
			{
				var userId = _currentUserService.GetUserId();
				var campaigns = _unitOfWork.GetRepository<CampaignMember>().Entities.Where(s => s.UserId == Guid.Parse(userId));

				var campaignList = campaigns.Select(c => new CampaignListResponse
				{
					Id = c.CampaignId,
					CampaignName = c.Campaign.CampaignName,
					Url = c.Campaign.Image,
					Status = c.Campaign.Status,
				});

				return BaseResponse<IEnumerable<CampaignListResponse>>.OkResponse(campaignList, "Campaign IDs and names retrieved successfully");
			}
			catch (Exception ex)
			{
				return BaseResponse<IEnumerable<CampaignListResponse>>.FailResponse(ex.Message);
			}
		}


		public async Task<BaseResponse<CampaignDetailResponse>> GetCampaignByIdAsync(string id)
		{
			try
			{
				var campaign = await _unitOfWork.GetRepository<Campaign>().Entities
						.Include(c => c.Category)
						.Include(c => c.CampaignPayoutModels)
						.ThenInclude(cpm => cpm.PayoutModel)
						.FirstOrDefaultAsync(s => s.Id == id);
				var userId = _currentUserService.GetUserId();
				var campaignMember = await _unitOfWork.GetRepository<CampaignMember>().Entities.FirstOrDefaultAsync(c => c.CampaignId == id && c.UserId == Guid.Parse(userId));

				if (campaign == null)
					return BaseResponse<CampaignDetailResponse>.FailResponse("Campaign not found");

				var response = _mapper.Map<CampaignDetailResponse>(campaign);

				if (campaignMember != null)
				{
					response.IsJoin = true;
				}
				else
				{
					response.IsJoin = false;
				}
				return BaseResponse<CampaignDetailResponse>.OkResponse(response, "Campaign retrieved successfully");
			}
			catch (Exception ex)
			{
				return BaseResponse<CampaignDetailResponse>.FailResponse(ex.Message);
			}
		}

		public async Task<BaseResponse<bool>> UpdateCampaignAsync(string id, CampaignRequest updatedCampaign)
		{
			try
			{
				var userId = _currentUserService.GetUserId();
				var campaign = await _unitOfWork.GetRepository<Campaign>().GetByIdAsync(id);
				if (campaign == null)
					return BaseResponse<bool>.FailResponse("Campaign not found");

				_mapper.Map(updatedCampaign, campaign);

				campaign.UserId = Guid.Parse(userId);

				_unitOfWork.GetRepository<Campaign>().Update(campaign);
				await _unitOfWork.SaveAsync();

				return BaseResponse<bool>.OkResponse(true, "Campaign updated successfully");
			}
			catch (Exception ex)
			{
				return BaseResponse<bool>.FailResponse(ex.Message);
			}
		}

		public async Task<BaseResponse<bool>> DeleteCampaignAsync(string id)
		{
			try
			{
				var campaign = await _unitOfWork.GetRepository<Campaign>().GetByIdAsync(id);
				if (campaign == null)
					return BaseResponse<bool>.FailResponse("Campaign not found");

				await _unitOfWork.GetRepository<Campaign>().DeleteAsync(id);
				await _unitOfWork.SaveAsync();

				return BaseResponse<bool>.OkResponse(true, "Campaign deleted successfully");
			}
			catch (Exception ex)
			{
				return BaseResponse<bool>.FailResponse(ex.Message);
			}
		}

		public async Task<BasePaginatedList<CampaignFilterResponse>> FilterCampaignsAsync(string? name, string? status, DateTime? startDate, DateTime? endDate, string? payoutMethod, string? category, int? index, int? size)
		{
			try
			{
				IQueryable<Campaign> query = _unitOfWork.GetRepository<Campaign>().Entities;

				if (!string.IsNullOrEmpty(payoutMethod))
				{
					List<string> campaignIdsWithPayout = await _unitOfWork.GetRepository<CampaignPayoutModel>()
						.Entities
						.Where(cp => cp.PayoutModelId == payoutMethod)
						.Select(cp => cp.CampaignId)
						.ToListAsync();

					query = query.Where(c => campaignIdsWithPayout.Contains(c.Id));
				}

				if (!string.IsNullOrEmpty(name))
				{
					query = query.Where(c => c.CampaignName.Contains(name));
				}

				if (!string.IsNullOrEmpty(status))
				{
					query = query.Where(c => c.Status == status);
				}

				if (startDate.HasValue)
				{
					query = query.Where(c => c.StartDate >= startDate.Value);
				}

				if (endDate.HasValue)
				{
					query = query.Where(c => c.EndDate <= endDate.Value);
				}

				if (!string.IsNullOrEmpty(category))
				{
					query = query.Where(c => c.CategoryId == category);
				}

				BasePaginatedList<Campaign> result = await _unitOfWork.GetRepository<Campaign>().GetPagging(query, index, size);
				List<CampaignFilterResponse> campaigns = _mapper.Map<List<CampaignFilterResponse>>(result.Items);
				return new BasePaginatedList<CampaignFilterResponse>(campaigns, result.TotalItems, result.CurrentPage, result.PageSize);
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		public async Task<BaseResponse<bool>> UpdateCampaignStatusAsync(string id, CampaignStatus status)
		{
			_unitOfWork.BeginTransaction();
			try
			{
				var campaign = await _unitOfWork.GetRepository<Campaign>().GetByIdAsync(id);
				if (campaign == null)
					return BaseResponse<bool>.FailResponse("Campaign not found");

				campaign.Status = status.ToString();

				_unitOfWork.GetRepository<Campaign>().Update(campaign);
				await _unitOfWork.SaveAsync();
				_unitOfWork.CommitTransaction();
				return BaseResponse<bool>.OkResponse(true, $"Campaign status updated to {campaign.Status}");
			}
			catch (Exception ex)
			{
				return BaseResponse<bool>.FailResponse(ex.Message);
			}
		}

		public async Task<BasePaginatedList<CampaignResponse>> GetWaitCampaignList(int? index, int? size)
		{
			try
			{
				IQueryable<Campaign> query = _unitOfWork.GetRepository<Campaign>().Entities.Where(c => c.Status == CampaignStatus.Wait.ToString());

				BasePaginatedList<Campaign> result = await _unitOfWork.GetRepository<Campaign>().GetPagging(query, index, size);
				List<CampaignResponse> campaigns = _mapper.Map<List<CampaignResponse>>(result.Items);
				return new BasePaginatedList<CampaignResponse>(campaigns, result.TotalItems, result.CurrentPage, result.PageSize);
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		public async Task<BasePaginatedList<CampaignFilterResponse>> GetAdvertisorCampaignsAsync(string? name, string? status, int? index, int? size)
		{
			try
			{
				var userId = _currentUserService.GetUserId();
				IQueryable<Campaign> query = _unitOfWork.GetRepository<Campaign>().Entities.Where(s => s.UserId == Guid.Parse(userId));

				if (!string.IsNullOrEmpty(name))
				{
					query = query.Where(c => c.CampaignName.Contains(name));
				}

				if (!string.IsNullOrEmpty(status))
				{
					query = query.Where(c => c.Status == status);
				}

				BasePaginatedList<Campaign> result = await _unitOfWork.GetRepository<Campaign>().GetPagging(query, index, size);
				List<CampaignFilterResponse> campaigns = _mapper.Map<List<CampaignFilterResponse>>(result.Items);
				return new BasePaginatedList<CampaignFilterResponse>(campaigns, result.TotalItems, result.CurrentPage, result.PageSize);
			}
			catch (Exception ex)
			{
				throw;
			}
		}
	}
}