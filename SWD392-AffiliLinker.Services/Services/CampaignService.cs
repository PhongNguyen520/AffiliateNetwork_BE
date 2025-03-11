using Microsoft.EntityFrameworkCore;
using SWD392_AffiliLinker.Repositories.Entities;
using SWD392_AffiliLinker.Repositories.IUOW;
using SWD392_AffiliLinker.Services.DTO;
using SWD392_AffiliLinker.Core.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SWD392_AffiliLinker.Services.Interfaces;
using SWD392_AffiliLinker.Services.DTO.AuthenDTO.Request;
using SWD392_AffiliLinker.Services.DTO.AuthenDTO.Response;

namespace SWD392_AffiliLinker.Services
{
	public class CampaignService : ICampaignService
	{
		private readonly IUnitOfWork _unitOfWork;

		public CampaignService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<BaseResponse<CampaignResponse>> CreateCampaignAsync(CampaignRequest campaignRequest)
		{
			try
			{

				var campaign = new Campaign
				{
					Id = Guid.NewGuid().ToString(),
					CampaignName = campaignRequest.CampaignName,
					Description = campaignRequest.Description,
					Introduction = campaignRequest.Introduction,
					Policy = campaignRequest.Policy,
					Image = campaignRequest.Image,
					WebsiteLink = campaignRequest.WebsiteLink,
					TargetCustomer = campaignRequest.TargetCustomer,
					Zone = campaignRequest.Zone,
					CategoryId = campaignRequest.CategoryId,
					StartDate = campaignRequest.StartDate,
					EndDate = campaignRequest.EndDate,
					Status = campaignRequest.Status,
					UserId = campaignRequest.UserId,
					EnrollCount = 0, // Initialize with default values
					ConversionRate = 0 // Initialize with default values
				};

				await _unitOfWork.GetRepository<Campaign>().InsertAsync(campaign);

				await _unitOfWork.SaveAsync();

				var response = new CampaignResponse
				{
					Id = campaign.Id,
					CampaignName = campaign.CampaignName,
					Description = campaign.Description,
					Introduction = campaign.Introduction,
					Policy = campaign.Policy,
					Image = campaign.Image,
					WebsiteLink = campaign.WebsiteLink,
					EnrollCount = campaign.EnrollCount,
					ConversionRate = campaign.ConversionRate,
					TargetCustomer = campaign.TargetCustomer,
					Zone = campaign.Zone,
					CategoryId = campaign.CategoryId,
					StartDate = campaign.StartDate,
					EndDate = campaign.EndDate,
					Status = campaign.Status,
					UserId = campaign.UserId
				};

				return BaseResponse<CampaignResponse>.OkResponse(response, "Campaign created successfully");
			}
			catch (Exception ex)
			{
				return BaseResponse<CampaignResponse>.FailResponse($"Error: {ex.Message} | Inner: {ex.InnerException?.Message}");
			}
		}


		public async Task<BaseResponse<IEnumerable<CampaignResponse>>> GetAllCampaignsAsync()
		{
			try
			{
				var campaigns = await _unitOfWork.GetRepository<Campaign>().GetAllAsync();
				var campaignResponses = campaigns.Select(c => new CampaignResponse
				{
					Id = c.Id,
					CampaignName = c.CampaignName,
					Description = c.Description,
					Introduction = c.Introduction,
					Policy = c.Policy,
					Image = c.Image,
					WebsiteLink = c.WebsiteLink,
					EnrollCount = c.EnrollCount,
					ConversionRate = c.ConversionRate,
					TargetCustomer = c.TargetCustomer,
					Zone = c.Zone,
					CategoryId = c.CategoryId,
					StartDate = c.StartDate,
					EndDate = c.EndDate,
					Status = c.Status,
					UserId = c.UserId

				});

				return BaseResponse<IEnumerable<CampaignResponse>>.OkResponse(campaignResponses, "Campaigns retrieved successfully");
			}
			catch (Exception ex)
			{
				return BaseResponse<IEnumerable<CampaignResponse>>.FailResponse(ex.Message);
			}
		}


		public async Task<BaseResponse<IEnumerable<CampaignListResponse>>> GetAllCampaignIdsAndNamesAsync()
		{
			try
			{
				var campaigns = await _unitOfWork.GetRepository<Campaign>().GetAllAsync();

				var campaignList = campaigns.Select(c => new CampaignListResponse
				{
					Id = c.Id,
					CampaignName = c.CampaignName
				});

				return BaseResponse<IEnumerable<CampaignListResponse>>.OkResponse(campaignList, "Campaign IDs and names retrieved successfully");
			}
			catch (Exception ex)
			{
				return BaseResponse<IEnumerable<CampaignListResponse>>.FailResponse(ex.Message);
			}
		}


		public async Task<BaseResponse<CampaignResponse>> GetCampaignByIdAsync(string id)
		{
			try
			{
				var campaign = await _unitOfWork.GetRepository<Campaign>().GetByIdAsync(id);
				if (campaign == null)
					return BaseResponse<CampaignResponse>.FailResponse("Campaign not found");

				var response = new CampaignResponse
				{
					Id = campaign.Id,
					CampaignName = campaign.CampaignName,
					Description = campaign.Description,
					Introduction = campaign.Introduction,
					Policy = campaign.Policy,
					Image = campaign.Image,
					WebsiteLink = campaign.WebsiteLink,
					EnrollCount = campaign.EnrollCount,
					ConversionRate = campaign.ConversionRate,
					TargetCustomer = campaign.TargetCustomer,
					Zone = campaign.Zone,
					CategoryId = campaign.CategoryId,
					StartDate = campaign.StartDate,
					EndDate = campaign.EndDate,
					Status = campaign.Status,
					UserId = campaign.UserId
				};

				return BaseResponse<CampaignResponse>.OkResponse(response, "Campaign retrieved successfully");
			}
			catch (Exception ex)
			{
				return BaseResponse<CampaignResponse>.FailResponse(ex.Message);
			}
		}

		public async Task<BaseResponse<bool>> UpdateCampaignAsync(string id, CampaignRequest updatedCampaign)
		{
			try
			{
				var campaign = await _unitOfWork.GetRepository<Campaign>().GetByIdAsync(id);
				if (campaign == null)
					return BaseResponse<bool>.FailResponse("Campaign not found");

				campaign.CampaignName = updatedCampaign.CampaignName;
				campaign.Description = updatedCampaign.Description;
				campaign.Introduction = updatedCampaign.Introduction;
				campaign.Policy = updatedCampaign.Policy;
				campaign.Image = updatedCampaign.Image;
				campaign.WebsiteLink = updatedCampaign.WebsiteLink;
				campaign.TargetCustomer = updatedCampaign.TargetCustomer;
				campaign.Zone = updatedCampaign.Zone;
				campaign.CategoryId = updatedCampaign.CategoryId;
				campaign.StartDate = updatedCampaign.StartDate;
				campaign.EndDate = updatedCampaign.EndDate;
				campaign.Status = updatedCampaign.Status;
				campaign.UserId = updatedCampaign.UserId;

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

		public async Task<BaseResponse<IEnumerable<CampaignResponse>>> FilterCampaignsAsync(string? name, string? status, DateTime? startDate, DateTime? endDate)
		{
			try
			{
				var query = _unitOfWork.GetRepository<Campaign>().GetAllQueryable();

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

				var campaigns = await query.ToListAsync();
				var campaignResponses = campaigns.Select(c => new CampaignResponse
				{
					Id = c.Id,
					CampaignName = c.CampaignName,
					Description = c.Description,
					Introduction = c.Introduction,
					Policy = c.Policy,
					Image = c.Image,
					WebsiteLink = c.WebsiteLink,
					EnrollCount = c.EnrollCount,
					ConversionRate = c.ConversionRate,
					TargetCustomer = c.TargetCustomer,
					Zone = c.Zone,
					CategoryId = c.CategoryId,
					StartDate = c.StartDate,
					EndDate = c.EndDate,
					Status = c.Status,
					UserId = c.UserId
				});

				return BaseResponse<IEnumerable<CampaignResponse>>.OkResponse(campaignResponses, "Campaigns filtered successfully");
			}
			catch (Exception ex)
			{
				return BaseResponse<IEnumerable<CampaignResponse>>.FailResponse(ex.Message);
			}
		}
	}
}