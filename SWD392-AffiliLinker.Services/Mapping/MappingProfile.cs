using AutoMapper;
using SWD392_AffiliLinker.Core.Base;
using SWD392_AffiliLinker.Repositories.Entities;
using SWD392_AffiliLinker.Services.DTO.AffiliateLinkDTO.Request;
using SWD392_AffiliLinker.Services.DTO.AffiliateLinkDTO.Response;
using SWD392_AffiliLinker.Services.DTO.AuthenDTO.Request;

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
		}
	}
}
