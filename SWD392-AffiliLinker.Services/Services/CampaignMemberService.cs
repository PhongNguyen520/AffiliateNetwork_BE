using SWD392_AffiliLinker.Core.Base;
using SWD392_AffiliLinker.Core.Store;
using SWD392_AffiliLinker.Core.Utils;
using SWD392_AffiliLinker.Repositories.Entities;
using SWD392_AffiliLinker.Repositories.IUOW;
using SWD392_AffiliLinker.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SWD392_AffiliLinker.Core.Store.EnumStatus;

namespace SWD392_AffiliLinker.Services.Services
{
	public class CampaignMemberService : ICampaignMemberService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly ICurrentUserService _currentUserService;

		public CampaignMemberService(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
		{
			_unitOfWork = unitOfWork;
			_currentUserService = currentUserService;
		}

		public async Task JoinCampaign(string campaignId)
		{
			_unitOfWork.BeginTransaction();
			try
			{
				var campaign = await  _unitOfWork.GetRepository<Campaign>().FindAsync(campaignId);

				if (campaign == null)
				{
					_unitOfWork.RollBack();
					throw new BaseException.ErrorException(StatusCodes.BadRequest, StatusCodes.BadRequest.Name(), "Campaign is not exist");
				}

				var userId = _currentUserService.GetUserId();
				if (userId == null)
				{
					_unitOfWork.RollBack();
					throw new BaseException.ErrorException(StatusCodes.Unauthorized, StatusCodes.Unauthorized.Name(), "Expired");
				}

				CampaignMember result = new CampaignMember()
				{
					CreatedTime = DateTime.UtcNow,
					LastUpdatedTime = DateTime.UtcNow,
					CampaignId = campaignId,
					UserId = Guid.Parse(userId),
					Status = MemberStatus.Active.ToString()
				};
				await _unitOfWork.GetRepository<CampaignMember>().InsertAsync(result);
				campaign.EnrollCount++;
				await _unitOfWork.GetRepository<Campaign>().UpdateAsync(campaign);
				await _unitOfWork.SaveAsync();
				_unitOfWork.CommitTransaction();
			}
			catch (Exception ex)
			{
				_unitOfWork.RollBack();
				throw;
			}
		}
	}
}
