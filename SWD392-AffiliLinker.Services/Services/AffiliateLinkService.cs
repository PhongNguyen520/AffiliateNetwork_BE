using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Logging;
using SWD392_AffiliLinker.Core.Base;
using SWD392_AffiliLinker.Core.Store;
using SWD392_AffiliLinker.Core.Utils;
using SWD392_AffiliLinker.Repositories.Entities;
using SWD392_AffiliLinker.Repositories.IUOW;
using SWD392_AffiliLinker.Services.Config.LinkConfig;
using SWD392_AffiliLinker.Services.DTO.AffiliateLinkDTO.Request;
using SWD392_AffiliLinker.Services.DTO.AffiliateLinkDTO.Response;
using SWD392_AffiliLinker.Services.DTO.ClickDTO.Response;
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
                string userId = _currentUserService.GetUserId();
                if (string.IsNullOrEmpty(userId))
                {
                    throw new BaseException.ErrorException(StatusCodes.Unauthorized, StatusCodes.Unauthorized.Name(), "Login to continue");
                }
                Campaign? campaign = await _unitOfWork.GetRepository<Campaign>().GetByIdAsync(request.CampaignId);
                if (campaign == null)
                {
                    throw new BaseException.ErrorException(StatusCodes.BadRequest, StatusCodes.BadRequest.Name(), "Campaign doesn't exist!");
                }
                request.Status = LinkStatus.Active;
                if (string.IsNullOrEmpty(request.Url))
                {
                    request.Url = campaign.WebsiteLink;
                }
                var result = _mapper.Map<AffiliateLink>(request);
                result.CreatedTime = DateTime.UtcNow;
                result.LastUpdatedTime = DateTime.UtcNow;
                result.UserId = Guid.Parse(userId);
                result.ShortenUrl = await GenerateShortCodeAsync();

                if (!string.IsNullOrEmpty(request.OptimizeUrl))
                {
                    var slug = request.OptimizeUrl.Replace(" ", "-").ToLower();
                    result.OptimizeUrl = await GenerateUniqueSlug(slug);
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

        public async Task<BasePaginatedList<GetLinksResponse>> GetPublisherLinkList(int? pageIndex, int? pageSize)
        {
            try
            {
                var userId = _currentUserService.GetUserId();
                if (string.IsNullOrEmpty(userId))
                {
                    throw new BaseException.ErrorException(StatusCodes.Unauthorized, StatusCodes.Unauthorized.Name(), "Login to continue");
                }
                var repository = _unitOfWork.GetRepository<AffiliateLink>();
                IQueryable<AffiliateLink>? query = repository.Entities.Where(s => s.UserId == Guid.Parse(userId));
                BasePaginatedList<AffiliateLink>? pagingList = await repository.GetPagging(query, pageIndex, pageSize);
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

		public async Task<BasePaginatedList<GetLinksResponse>> GetLinkListByCampaignId(FilterLinkRequest request)
		{
			try
			{
				var userId = _currentUserService.GetUserId();
				if (string.IsNullOrEmpty(userId))
				{
					throw new BaseException.ErrorException(StatusCodes.Unauthorized, StatusCodes.Unauthorized.Name(), "Login to continue");
				}
				var repository = _unitOfWork.GetRepository<AffiliateLink>();
				IQueryable<AffiliateLink>? query = repository.Entities.Where(s => s.CampaignId == request.id && s.UserId == Guid.Parse(userId));
				BasePaginatedList<AffiliateLink>? pagingList = await repository.GetPagging(query, request.PageIndex, request.PageSize);
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

		public async Task<AffiLinkClickResponse> RedirectShortenUrl(string? shortenCode)
        {
            try
            {
                var affiliateLink = await _unitOfWork.GetRepository<AffiliateLink>()
                                              .Entities
                                              .FirstOrDefaultAsync(s => s.ShortenUrl == shortenCode);
                if (affiliateLink is null)
                { 
                    throw new BaseException.ErrorException(StatusCodes.NotFound, StatusCodes.NotFound.Name(), "AffiliateLink Not Exist!!!"); 
                }

                return new AffiLinkClickResponse
                {
                    Id = affiliateLink.Id,
                    Url = affiliateLink.Url,
                };
            }
            catch
            {
                throw;
            }
        }

        public async Task<AffiLinkClickResponse> RedirectOptimizeUrl(string? slug)
        {
            try
            {
                var affiliateLink = await _unitOfWork.GetRepository<AffiliateLink>()
                                           .Entities
                                           .FirstOrDefaultAsync(s => s.OptimizeUrl == slug);
                if (affiliateLink is null)
                {
                    throw new BaseException.ErrorException(StatusCodes.NotFound, StatusCodes.NotFound.Name(), "AffiliateLink Not Exist!!!");
                }

                return new AffiLinkClickResponse
                {
                    Id = affiliateLink.Id,
                    Url = affiliateLink.Url,
                };
            }
            catch
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

        public async Task<string> GenerateUniqueSlug(string slug)
        {
            var existingSlugs = await _unitOfWork.GetRepository<AffiliateLink>()
                .Entities
                .Where(s => s.OptimizeUrl.StartsWith(slug))
                .Select(s => s.OptimizeUrl)
                .ToListAsync();

            if (!existingSlugs.Contains(slug))
                return slug;

            int count = 1;
            string newSlug;
            do
            {
                newSlug = $"{slug}-{count++}";
            } while (existingSlugs.Contains(newSlug));

            return newSlug;
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
