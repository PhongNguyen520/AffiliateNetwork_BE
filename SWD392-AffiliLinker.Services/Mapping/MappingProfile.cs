using AutoMapper;
using SWD392_AffiliLinker.Repositories.Entities;
using SWD392_AffiliLinker.Services.DTO.AuthenDTO.Request;

namespace SWD392_AffiliLinker.Services.Mapping
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			CreateMap<User, RegisterRequest>().ReverseMap();
		}

	}
}
