using AutoMapper;
using SWD392_AffiliLinker.Core.Base;
using SWD392_AffiliLinker.Repositories.Entities;
using SWD392_AffiliLinker.Services.DTO.AccountDTO.Response;
using SWD392_AffiliLinker.Services.DTO.AffiliateLinkDTO.Request;
using SWD392_AffiliLinker.Services.DTO.AffiliateLinkDTO.Response;
using SWD392_AffiliLinker.Services.DTO.AuthenDTO.Request;
using SWD392_AffiliLinker.Services.DTO.CampaginDTO.Request;
using SWD392_AffiliLinker.Services.DTO.CampaginDTO.Response;
using SWD392_AffiliLinker.Services.DTO.CategoryDTO.Request;
using SWD392_AffiliLinker.Services.DTO.CategoryDTO.Response;
using SWD392_AffiliLinker.Services.DTO.ClickDTO.Request;
using SWD392_AffiliLinker.Services.DTO.ClickDTO.Response;
using SWD392_AffiliLinker.Services.DTO.CommissionDTO.Response;
using SWD392_AffiliLinker.Services.DTO.ConversionDTO.Request;
using SWD392_AffiliLinker.Services.DTO.ConversionDTO.Response;
using SWD392_AffiliLinker.Services.DTO.PayoutModelDTO.Request;
using SWD392_AffiliLinker.Services.DTO.PayoutModelDTO.Response;

namespace SWD392_AffiliLinker.Services.Mapping
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			CreateMap<User, RegisterRequest>().ReverseMap();


			CreateMap<User, AdvertiserRegisterRequest>()
				.ForMember(dest => dest.AdvertiserRequest, opt => opt.MapFrom(src => src.Advertiser))
				.ReverseMap();
			CreateMap<Advertiser, AdvertiserRequest>().ReverseMap();

			CreateMap<User, PublisherRegisterRequest>()
				.ForMember(dest => dest.PublisherRequest, opt => opt.MapFrom(src => src.Publisher))
				.ReverseMap();
			CreateMap<Publisher, PublisherRequest>().ReverseMap();
			CreateMap<AffiliateLink, CreateLinkRequest>()
				.ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status)).ReverseMap();
			CreateMap<AffiliateLink, GetLinksResponse>();
			CreateMap<Campaign, CampaignRequest>();
			CreateMap<Campaign, CampaignResponse>();
			CreateMap<Campaign, CreateCampaignRequest>()
				.ForMember(dest => dest.PayoutModelsId, opt => opt.MapFrom(src => src.CampaignPayoutModels.Select(cpm => cpm.PayoutModelId.ToString()).ToList()));
			CreateMap<CreateCampaignRequest, Campaign>()
				.ForMember(dest => dest.Id, opt => opt.Ignore())
				.ForMember(dest => dest.EnrollCount, opt => opt.Ignore())
				.ForMember(dest => dest.Status, opt => opt.Ignore())
				.ForMember(dest => dest.UserId, opt => opt.Ignore())
				.ForMember(dest => dest.CampaignPayoutModels, opt => opt.Ignore())
				.ForMember(dest => dest.CampaignMembers, opt => opt.Ignore())
				.ForMember(dest => dest.AffiliateLinks, opt => opt.Ignore());
			CreateMap<Campaign, CampaignDetailResponse>()
				.ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
				.ForMember(dest => dest.PayoutModelName, opt => opt.MapFrom(src => src.CampaignPayoutModels
				.Select(cpm => cpm.PayoutModel.Name).ToList()));
			CreateMap<Campaign, CampaignFilterResponse>();
			CreateMap<PayoutModel, CreatePayoutModelRequest>().ReverseMap();
			CreateMap<PayoutModel, GetPayoutModelsResponse>().ReverseMap();
			CreateMap<CampaignCategory, CreateCategoryRequest>().ReverseMap();
			CreateMap<CampaignCategory, GetCategoriesResponse>().ReverseMap();
			CreateMap<ClickInfo, ClickInfoRequest>().ReverseMap();
			CreateMap<ClickInfo, TotalClickInfoResponse>().ReverseMap();
			CreateMap<ClickInfo, ExcelClickInfoResponse>().ReverseMap();
			CreateMap<BasePaginatedList<ClickInfo>, BasePaginatedList<TotalClickInfoResponse>>().ReverseMap();
			CreateMap<Conversion, CreateConversion>().ReverseMap();
			CreateMap<Conversion, ConversionResponse>().ReverseMap();
			CreateMap<BasePaginatedList<Conversion>, BasePaginatedList<ConversionResponse>>().ReverseMap();
			CreateMap<User, AccountResponse>().ReverseMap();
			CreateMap<Publisher, PublisherAccountResponse>()
				.ForMember(dest => dest.PublisherId, opt => opt.MapFrom(src => src.Id)).ReverseMap()
				.ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id)).ReverseMap()
				.ReverseMap();
			CreateMap<Advertiser, AdvertiserAccountResponse>()
                .ForMember(dest => dest.AdvertiserId, opt => opt.MapFrom(src => src.Id)).ReverseMap()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id)).ReverseMap()
                .ReverseMap();
            CreateMap<BasePaginatedList<User>, BasePaginatedList<AccountResponse>>().ReverseMap();
			CreateMap<Commission, CommissionResponse>().ReverseMap();
        }
	}
}
