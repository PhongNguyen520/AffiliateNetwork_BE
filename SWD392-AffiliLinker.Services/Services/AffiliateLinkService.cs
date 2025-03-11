using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SWD392_AffiliLinker.Core.Base;
using SWD392_AffiliLinker.Core.Store;
using SWD392_AffiliLinker.Core.Utils;
using SWD392_AffiliLinker.Repositories.Entities;
using SWD392_AffiliLinker.Repositories.IUOW;
using SWD392_AffiliLinker.Services.Config.LinkConfig;
using SWD392_AffiliLinker.Services.DTO.AffiliateLinkDTO.Request;
using SWD392_AffiliLinker.Services.DTO.AffiliateLinkDTO.Response;
using SWD392_AffiliLinker.Services.Interfaces;
using static SWD392_AffiliLinker.Core.Store.EnumStatus;

namespace SWD392_AffiliLinker.Services.Services
{
	public class AffiliateLinkService : IAffiliateLinkService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly ICurrentUserService _currentUserService;
		private readonly IOptions<AffiliateDomainOptions> _options;
		private readonly IMapper _mapper;

		public AffiliateLinkService(IUnitOfWork unitOfWork, ICurrentUserService currentUserService, IMapper mapper, IOptions<AffiliateDomainOptions> options)
		{
			_unitOfWork = unitOfWork;
			_currentUserService = currentUserService;
			_mapper = mapper;
			_options = options;
		}

		public async Task<CreateLinkResponse> CreateLink(CreateLinkRequest request)
		{
			_unitOfWork.BeginTransaction();
			try
			{
				var userId = _currentUserService.GetUserId();
				if (string.IsNullOrEmpty(userId))
				{
					_unitOfWork.RollBack();
					throw new BaseException.ErrorException(StatusCodes.BadRequest, StatusCodes.BadRequest.Name(), "Exprier Time");
				}
				var campaign = await _unitOfWork.GetRepository<Campaign>().GetByIdAsync(request.CampaignId);
				request.Status = LinkStatus.Active;
				if (string.IsNullOrEmpty(request.Url))
				{
					request.Url = campaign.WebsiteLink;
				}
				var result = _mapper.Map<AffiliateLink>(request);
				result.UserId = Guid.Parse(userId);
				result.ShortenUrl = await GenerateShortCodeAsync();

				if (!string.IsNullOrEmpty(request.OptimizeUrl))
				{
					result.OptimizeUrl = request.OptimizeUrl.Replace(" ", "-").ToLower();
				}
				await _unitOfWork.GetRepository<AffiliateLink>().InsertAsync(result);
				await _unitOfWork.SaveAsync();
				_unitOfWork.CommitTransaction();
				return new CreateLinkResponse
				{
					UrlShorten = GetShortenUrl(result.ShortenUrl),
					UrlOptimize = GetOptimizeUrl(result.OptimizeUrl),
					CampaignName = campaign.CampaignName,
					Introduction = campaign.Introduction,
					Description = campaign.Description,
				};
			}
			catch (Exception ex)
			{
				_unitOfWork.RollBack();
				throw;
			}
		}

		public async Task<BasePaginatedList<GetLinksResponse>> GetPublisherLinkList(int pageIndex)
		{
			try
			{
				var userId = _currentUserService.GetUserId();
				if (string.IsNullOrEmpty(userId))
				{
					_unitOfWork.RollBack();
					throw new BaseException.ErrorException(StatusCodes.BadRequest, StatusCodes.BadRequest.Name(), "Exprier Time");
				}
				var repository = _unitOfWork.GetRepository<AffiliateLink>();
				IQueryable<AffiliateLink>? query = repository.Entities.Where(s => s.UserId == Guid.Parse(userId));
				BasePaginatedList<AffiliateLink>? pagingList = await repository.GetPagging(query, pageIndex, 5);
				List<GetLinksResponse> result = _mapper.Map<List<GetLinksResponse>>(pagingList.Items);
				foreach (var link in result)
				{
					link.ShortenUrl = GetShortenUrl(link.ShortenUrl);
					link.OptimizeUrl = GetOptimizeUrl(link.OptimizeUrl);
				}
				return new BasePaginatedList<GetLinksResponse>(result, pagingList.TotalItems, pagingList.CurrentPage, pagingList.PageSize);
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		public string GetShortenUrl(string shortCode) => $"{_options.Value.ShortenDomain}{shortCode}";
		public string GetOptimizeUrl(string optimizeCode) => $"{_options.Value.OptimizeDomain}{optimizeCode}";
		public async Task<string> GenerateShortCodeAsync()
		{
			string shortCode;
			do
			{
				shortCode = GenerateBase62Code(8);
			} while (await _unitOfWork.GetRepository<AffiliateLink>().SearchAsync(l => l.ShortenUrl == shortCode) is { Count: > 0 });

			return shortCode;
		}

		private static string GenerateBase62Code(int length)
		{
			const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
			var bytes = Guid.NewGuid().ToByteArray();
			var value = BitConverter.ToUInt64(bytes, 0);
			var shortCode = new char[length];

			for (int i = 0; i < length; i++)
			{
				shortCode[i] = chars[(int)(value % (ulong)chars.Length)];
				value /= (ulong)chars.Length;
			}

			return new string(shortCode);
		}
	}
}
