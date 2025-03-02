using SWD392_AffiliLinker.Core.Base;
using SWD392_AffiliLinker.Services.DTO.AuthenDTO.Request;
using SWD392_AffiliLinker.Services.DTO.AuthenDTO.Response;

namespace SWD392_AffiliLinker.Services.Interfaces
{
	public interface IAuthenticationService
	{
		Task<BaseResponse<string>> RegisterAsync(RegisterRequest registerModelView);

		Task<AuthenResponse> Login(LoginRequest loginModel);
	}
}
