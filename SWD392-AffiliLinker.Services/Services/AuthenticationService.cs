using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SWD392_AffiliLinker.Core.Base;
using SWD392_AffiliLinker.Core.Utils;
using SWD392_AffiliLinker.Repositories.Entities;
using SWD392_AffiliLinker.Services.DTO.AuthenDTO.Request;
using SWD392_AffiliLinker.Services.DTO.AuthenDTO.Response;
using SWD392_AffiliLinker.Services.Interfaces;
using StatusCodes = SWD392_AffiliLinker.Core.Store.StatusCodes;

namespace SWD392_AffiliLinker.Services.Services
{
	public class AuthenticationService : IAuthenticationService
	{
		private readonly UserManager<User> _userManager;
		private readonly RoleManager<IdentityRole<Guid>> _roleManager;
		private readonly SignInManager<User> _signInManager;
		private readonly IJwtTokenService _jwtTokenService;
		private readonly IMapper _mapper;

		public AuthenticationService(UserManager<User> userManager, RoleManager<IdentityRole<Guid>> roleManager, SignInManager<User> signInManager, IJwtTokenService jwtTokenService, IMapper mapper)
		{
			_userManager = userManager;
			_roleManager = roleManager;
			_signInManager = signInManager;
			_jwtTokenService = jwtTokenService;
			_mapper = mapper;
		}

		private async Task<string> GenerateRefreshToken(User user)
		{
			string? refreshToken = Guid.NewGuid().ToString();

			string? initToken = await _userManager.GetAuthenticationTokenAsync(user, "Default", "RefreshToken");
			if (initToken != null)
			{

				await _userManager.RemoveAuthenticationTokenAsync(user, "Default", "RefreshToken");

			}

			await _userManager.SetAuthenticationTokenAsync(user, "Default", "RefreshToken", refreshToken);
			return refreshToken;
		}


		public async Task<BaseResponse<string>> RegisterAsync(RegisterRequest registerModelView)
		{
			User? user = await _userManager.FindByNameAsync(registerModelView.UserName);

			if (user != null)
			{
				throw new BaseException.ErrorException(StatusCodes.BadRequest, StatusCodes.BadRequest.Name(), "Email đã tồn tại");
			}

			if (await _userManager.Users.AnyAsync(u => u.UserName == registerModelView.UserName))
			{
				throw new BaseException.ErrorException(StatusCodes.BadRequest,
					StatusCodes.BadRequest.Name(), "Username is existed!");
			}

			if (await _userManager.Users.AnyAsync(u => u.Email == registerModelView.Email))
			{
				throw new BaseException.ErrorException(StatusCodes.BadRequest,
					StatusCodes.BadRequest.Name(), "Email is existed!");
			}

			if (await _userManager.Users.AnyAsync(u => u.PhoneNumber == registerModelView.PhoneNumber))
			{
				throw new BaseException.ErrorException(StatusCodes.BadRequest,
					StatusCodes.BadRequest.Name(), "PhoneNumber is existed!");
			}

			User? newUser = _mapper.Map<User>(registerModelView);

			IdentityResult? result = await _userManager.CreateAsync(newUser, registerModelView.Password);
			if (result.Succeeded)
			{
				bool roleExist = await _roleManager.RoleExistsAsync("Member");
				if (!roleExist)
				{
					await _roleManager.CreateAsync(new IdentityRole<Guid> { Name = "Member" });
				}
				await _userManager.AddToRoleAsync(newUser, "Member");

			}
			else
			{
				var errors = string.Join(", ", result.Errors.Select(e => e.Description)); throw new BaseException.ErrorException(StatusCodes.BadRequest, StatusCodes.BadRequest.Name(), errors);
			}
			return BaseResponse<string>.OkResponse(newUser.Id.ToString());
		}

		public async Task<AuthenResponse> Login(LoginRequest loginModel)
		{
			User? user = await _userManager.FindByNameAsync(loginModel.UserName)
			 ?? throw new BaseException.ErrorException(StatusCodes.NotFound, StatusCodes.NotFound.Name(), "Không tìm thấy user"); // 404

			if (user.DeletedTime.HasValue)
			{
				throw new BaseException.ErrorException(StatusCodes.BadRequest, StatusCodes.BadRequest.Name(), "Tài khoản đã bị xóa");
			}
			//if (!await _userManager.IsEmailConfirmedAsync(user))
			//{
			//	throw new BaseException.ErrorException(StatusCodes.BadRequest, ErrorCode.BadRequest, "Tài khoản chưa được xác nhận");
			//}
			SignInResult result = await _signInManager.PasswordSignInAsync(loginModel.UserName, loginModel.Password, false, false);
			if (!result.Succeeded)
			{
				throw new BaseException.ErrorException(StatusCodes.Unauthorized, StatusCodes.Unauthorized.Name(), "Mật khẩu không đúng");
			}

			string token = await _jwtTokenService.GenerateJwtToken(user);
			string refreshToken = await GenerateRefreshToken(user);
			var roles = await _userManager.GetRolesAsync(user);
			return new AuthenResponse
			{
				AccessToken = token,
				RefreshToken = refreshToken,
			};

		}
	}
}
