using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SWD392_AffiliLinker.Core.Base;
using SWD392_AffiliLinker.Core.Store;
using SWD392_AffiliLinker.Core.Utils;
using SWD392_AffiliLinker.Repositories.Entities;
using SWD392_AffiliLinker.Repositories.IUOW;
using SWD392_AffiliLinker.Services.DTO.AuthenDTO.Request;
using SWD392_AffiliLinker.Services.DTO.AuthenDTO.Response;
using SWD392_AffiliLinker.Services.Interfaces;
using System.IO;
using static SWD392_AffiliLinker.Core.Store.EnumStatus;
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
		private readonly IUnitOfWork _unitOfWork;
        private readonly IHepperUploadImage _hepperUploadImage;

		public AuthenticationService(UserManager<User> userManager, RoleManager<IdentityRole<Guid>> roleManager, SignInManager<User> signInManager, IJwtTokenService jwtTokenService, IMapper mapper, IUnitOfWork unitOfWork, IHepperUploadImage hepperUploadImage)
		{
			_userManager = userManager;
			_roleManager = roleManager;
			_signInManager = signInManager;
			_jwtTokenService = jwtTokenService;
			_mapper = mapper;
			_unitOfWork = unitOfWork;
            _hepperUploadImage = hepperUploadImage;
		}


        public async Task<string> RegisterPublisherAsync(PublisherRegisterRequest registerModelView)
        {
			_unitOfWork.BeginTransaction();
            try
            {
                #region Duplicate Check

                User? user = await _userManager.FindByEmailAsync(registerModelView.Email);

                if (user != null)
                {
                    throw new BaseException.ErrorException(StatusCodes.BadRequest, StatusCodes.BadRequest.Name(), "Email is existed!");
                }

                user = await _userManager.FindByNameAsync(registerModelView.UserName);

                if (user != null)
                {
                    throw new BaseException.ErrorException(StatusCodes.BadRequest, StatusCodes.BadRequest.Name(), "UserName is existed!");
                }

                if (await _userManager.Users.AnyAsync(u => u.PhoneNumber == registerModelView.PhoneNumber))
                {
                    throw new BaseException.ErrorException(StatusCodes.BadRequest,
                        StatusCodes.BadRequest.Name(), "PhoneNumber is existed!");
                }
                #endregion

                User? newUser = _mapper.Map<User>(registerModelView);

                newUser.Status = UserStatus.Active.ToString();
                newUser.CreatedTime = DateTime.Now;
                newUser.LastUpdatedTime = newUser.CreatedTime;
                newUser.Publisher.LastUpdatedTime = newUser.CreatedTime;
                newUser.Publisher.CreatedTime = newUser.CreatedTime;

                IdentityResult? result = await _userManager.CreateAsync(newUser, registerModelView.Password);
                if (result.Succeeded)
                {
                    bool roleExist = await _roleManager.RoleExistsAsync("Publisher");
                    if (!roleExist)
                    {
                        await _roleManager.CreateAsync(new IdentityRole<Guid> { Name = "Publisher" });
                    }
                    await _userManager.AddToRoleAsync(newUser, "Publisher");

                }
                else
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    throw new BaseException.ErrorException(StatusCodes.BadRequest, StatusCodes.BadRequest.Name(), errors);
                }
                await _unitOfWork.SaveAsync();
                _unitOfWork.CommitTransaction();
                return newUser.Id.ToString();
            }
            catch
            {
                _unitOfWork.RollBack();
                throw;
            }

        }


        public async Task<string> RegisterAdvertiserAsync(AdvertiserRegisterRequest registerModelView)
        {

            _unitOfWork.BeginTransaction();
            try
            {
                #region Duplicate Check

                User? user = await _userManager.FindByEmailAsync(registerModelView.Email);

                if (user != null)
                {
                    throw new BaseException.ErrorException(StatusCodes.BadRequest, StatusCodes.BadRequest.Name(), "Email is existed!");
                }

                user = await _userManager.FindByNameAsync(registerModelView.UserName);

                if (user != null)
                {
                    throw new BaseException.ErrorException(StatusCodes.BadRequest, StatusCodes.BadRequest.Name(), "UserName is existed!");
                }

                if (await _userManager.Users.AnyAsync(u => u.PhoneNumber == registerModelView.PhoneNumber))
                {
                    throw new BaseException.ErrorException(StatusCodes.BadRequest,
                        StatusCodes.BadRequest.Name(), "PhoneNumber is existed!");
                }
                #endregion

                User? newUser = _mapper.Map<User>(registerModelView);

                newUser.Status = UserStatus.Active.ToString();
                newUser.CreatedTime = DateTime.Now;
                newUser.LastUpdatedTime = newUser.CreatedTime;
                newUser.Advertiser.LastUpdatedTime = newUser.CreatedTime;
                newUser.Advertiser.CreatedTime = newUser.CreatedTime;

                IdentityResult? result = await _userManager.CreateAsync(newUser, registerModelView.Password);
                if (result.Succeeded)
                {
                    bool roleExist = await _roleManager.RoleExistsAsync("Advertiser");
                    if (!roleExist)
                    {
                        await _roleManager.CreateAsync(new IdentityRole<Guid> { Name = "Advertiser" });
                    }
                    await _userManager.AddToRoleAsync(newUser, "Advertiser");

                }
                else
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    throw new BaseException.ErrorException(StatusCodes.BadRequest, StatusCodes.BadRequest.Name(), errors);
                }
                await _unitOfWork.SaveAsync();
                _unitOfWork.CommitTransaction();
                return newUser.Id.ToString();
            }
            catch
            {
                _unitOfWork.RollBack();
                throw;
            }
        }


		public async Task<string> RegisterAdmin (RegisterRequest registerModelView)
		{
            _unitOfWork.BeginTransaction();
			try
			{
                #region Duplicate Check

                User? user = await _userManager.FindByEmailAsync(registerModelView.Email);

                if (user != null)
                {
                    throw new BaseException.ErrorException(StatusCodes.BadRequest, StatusCodes.BadRequest.Name(), "Email is existed!");
                }

                user = await _userManager.FindByNameAsync(registerModelView.UserName);

                if (user != null)
                {
                    throw new BaseException.ErrorException(StatusCodes.BadRequest, StatusCodes.BadRequest.Name(), "UserName is existed!");
                }

                if (await _userManager.Users.AnyAsync(u => u.PhoneNumber == registerModelView.PhoneNumber))
                {
                    throw new BaseException.ErrorException(StatusCodes.BadRequest,
                        StatusCodes.BadRequest.Name(), "PhoneNumber is existed!");
                }
                #endregion

				var newUser = _mapper.Map<User>(registerModelView);

                newUser.Status = UserStatus.Active.ToString();
                newUser.CreatedTime = DateTime.Now;
                newUser.LastUpdatedTime = newUser.CreatedTime;

                var result = await _userManager.CreateAsync(newUser, registerModelView.Password);
                if (result.Succeeded)
                {
                    bool roleExist = await _roleManager.RoleExistsAsync("Admin");
                    if (!roleExist)
                    {
                        await _roleManager.CreateAsync(new IdentityRole<Guid> { Name = "Admin" });
                    }
                    await _userManager.AddToRoleAsync(newUser, "Admin");

                }
                else
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    throw new BaseException.ErrorException(StatusCodes.BadRequest, StatusCodes.BadRequest.Name(), errors);
                }

                await _unitOfWork.SaveAsync();
                _unitOfWork.CommitTransaction();
                return newUser.Id.ToString();

            }
            catch (Exception ex)
			{
				_unitOfWork.RollBack();
				throw;
			}
		}


        public async Task<AuthenResponse> Login(LoginRequest loginModel)
		{
            try
            {
                User? user = await _userManager.FindByNameAsync(loginModel.UserName)
             ?? throw new BaseException.ErrorException(StatusCodes.NotFound, StatusCodes.NotFound.Name(), "User not exist!!!"); // 404

                if (user.DeletedTime.HasValue)
                {
                    throw new BaseException.ErrorException(StatusCodes.BadRequest, StatusCodes.BadRequest.Name(), "User is deleted!!!");
                }

                if (user.Status != UserStatus.Active.ToString())
                {
                    throw new BaseException.ErrorException(StatusCodes.BadRequest, StatusCodes.NotFound.Name(), "Account not activated!!!");
                }

                SignInResult result = await _signInManager.PasswordSignInAsync(loginModel.UserName, loginModel.Password, false, false);
                if (!result.Succeeded)
                {
                    throw new BaseException.ErrorException(StatusCodes.Unauthorized, StatusCodes.Unauthorized.Name(), "Password is wrong!!!");
                }

                string token = await _jwtTokenService.GenerateJwtToken(user);
                string refreshToken = await _jwtTokenService.GenerateRefreshToken(user);

                return new AuthenResponse
                {
                    AccessToken = token,
                    RefreshToken = refreshToken,
                };
            }
            catch
            {
                throw;
            }
		}
	}
}
