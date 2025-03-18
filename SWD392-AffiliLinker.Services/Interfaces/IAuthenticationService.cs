using Microsoft.AspNetCore.Http;
using SWD392_AffiliLinker.Core.Base;
using SWD392_AffiliLinker.Core.Store;
using SWD392_AffiliLinker.Services.DTO.AuthenDTO.Request;
using SWD392_AffiliLinker.Services.DTO.AuthenDTO.Response;

namespace SWD392_AffiliLinker.Services.Interfaces
{
	public interface IAuthenticationService
	{
		Task<string> RegisterPublisherAsync(PublisherRegisterRequest registerModelView);

		Task<string> RegisterAdvertiserAsync(AdvertiserRegisterRequest registerModelView);

		Task<string> RegisterAdmin(RegisterRequest registerModelView);

        Task<AuthenResponse> Login(LoginRequest loginModel);
	}
}
