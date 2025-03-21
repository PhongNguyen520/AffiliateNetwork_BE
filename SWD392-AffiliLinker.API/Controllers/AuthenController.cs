using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SWD392_AffiliLinker.Core.Base;
using SWD392_AffiliLinker.Core.Store;
using SWD392_AffiliLinker.Core.Utils;
using SWD392_AffiliLinker.Repositories.IUOW;
using SWD392_AffiliLinker.Services.DTO.AuthenDTO.Request;
using SWD392_AffiliLinker.Services.DTO.AuthenDTO.Response;
using SWD392_AffiliLinker.Services.Interfaces;

namespace SWD392_AffiliLinker.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly ILogger<AuthController> _logger;
        private readonly ICurrentUserService _currentUserService;
        private readonly IHepperUploadImage _hepperUploadImage;

        public AuthController(IAuthenticationService authenticationService, ILogger<AuthController> logger, ICurrentUserService currentUserService, IHepperUploadImage hepperUploadImage)
        {
            _authenticationService = authenticationService;
            _logger = logger;
            _currentUserService = currentUserService;
            _hepperUploadImage = hepperUploadImage;
        }


        #region Register
        //[AllowAnonymous]
        //[HttpPost("register")]
        //public async Task<ActionResult<BaseResponse<string>>> Register(RegisterRequest registerModel, [FromQuery] EnumRolesRegister roleModel)
        //{
        //	try
        //	{
        //		//var result = await _authenticationService.RegisterAsync(registerModelView);
        //		//if (result.StatusCode == Core.Store.StatusCodes.BadRequest)
        //		//{
        //		//	_unitOfWork.RollBack();
        //		//	return BadRequest(result);
        //		//}
        //		//return Ok(result);

        //		if(roleModel == EnumRolesRegister.Advertiser)
        //		{
        //			var result = await _authenticationService.RegisterAdvertiserAsync(registerModel);
        //		}
        //	}
        //	catch (BaseException.ErrorException ex)
        //	{
        //              return StatusCode((int)ex.StatusCode, new BaseResponse<string>
        //              {
        //                  StatusCode = ex.StatusCode,
        //                  Message = ex.ErrorDetail.ErrorMessage.ToString(),
        //                  Code = ex.StatusCode.Name()
        //              });
        //          }
        //	catch (Exception ex)
        //	{
        //		_logger.LogError(ex, "Lỗi xảy ra khi đăng ký tài khoản.");
        //		return StatusCode(500, new BaseResponse<string>
        //		{
        //			StatusCode = Core.Store.StatusCodes.ServerError,
        //			Message = "Có lỗi xảy ra, vui lòng thử lại sau."
        //		});
        //	}

        //}
        #endregion

        [AllowAnonymous]
        [HttpPost("register/advertiser")]
        public async Task<ActionResult<BaseResponse<string>>> RegisterWithAdvertiserRole(AdvertiserRegisterRequest advertiser)
        {
            try
            {
                var result = await _authenticationService.RegisterAdvertiserAsync(advertiser);
                return StatusCode((int)Core.Store.StatusCodes.OK,
                                   BaseResponse<string>.OkResponse(result, "Successfull"));
            }
            catch (BaseException.ErrorException ex)
            {
                return StatusCode((int)ex.StatusCode, new BaseResponse<string>
                {
                    StatusCode = ex.StatusCode,
                    Message = ex.ErrorDetail.ErrorMessage.ToString(),
                    Code = (int)ex.StatusCode
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi xảy ra khi đăng ký tài khoản.");
                return StatusCode(500, new BaseResponse<string>
                {
                    StatusCode = Core.Store.StatusCodes.ServerError,
                    Message = "Có lỗi xảy ra, vui lòng thử lại sau.",
                    Code = (int)Core.Store.StatusCodes.ServerError
                });
            }
        }

        [AllowAnonymous]
        [HttpPost("register/publisher")]
        public async Task<ActionResult<BaseResponse<string>>> RegisterWithPublisherRole(PublisherRegisterRequest publisher)
        {
            try
            {
                var result = await _authenticationService.RegisterPublisherAsync(publisher);
                return StatusCode((int)Core.Store.StatusCodes.OK,
                                  BaseResponse<string>.OkResponse(result, "Successfull"));
            }
            catch (BaseException.ErrorException ex)
            {
                return StatusCode((int)ex.StatusCode, new BaseResponse<string>
                {
                    StatusCode = ex.StatusCode,
                    Message = ex.ErrorDetail.ErrorMessage.ToString(),
                    Code = (int)ex.StatusCode
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi xảy ra khi đăng ký tài khoản.");
                return StatusCode(500, new BaseResponse<string>
                {
                    StatusCode = Core.Store.StatusCodes.ServerError,
                    Message = "Có lỗi xảy ra, vui lòng thử lại sau.",
                    Code = (int)Core.Store.StatusCodes.ServerError
                });
            }
        }

        [AllowAnonymous]
        [HttpPost("register/admin")]
        public async Task<ActionResult<BaseResponse<string>>> RegisterWithadminRole(RegisterRequest adminModel)
        {
            try
            {
                var result = await _authenticationService.RegisterAdmin(adminModel);
                return StatusCode((int)Core.Store.StatusCodes.OK,
                                  BaseResponse<string>.OkResponse(result, "Successfull"));
            }
            catch (BaseException.ErrorException ex)
            {
                return StatusCode((int)ex.StatusCode, new BaseResponse<string>
                {
                    StatusCode = ex.StatusCode,
                    Message = ex.ErrorDetail.ErrorMessage.ToString(),
                    Code = (int)ex.StatusCode
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi xảy ra khi đăng ký tài khoản.");
                return StatusCode(500, new BaseResponse<string>
                {
                    StatusCode = Core.Store.StatusCodes.ServerError,
                    Message = "Có lỗi xảy ra, vui lòng thử lại sau.",
                    Code = (int)Core.Store.StatusCodes.ServerError
                });
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<BaseResponse<AuthenResponse>>> Login(LoginRequest model)
        {
            try
            {
                var result = await _authenticationService.Login(model);
                return StatusCode((int)Core.Store.StatusCodes.OK,
                                  BaseResponse<AuthenResponse>.OkResponse(result, "Successfull"));
            }
            catch (BaseException.ErrorException ex)
            {
                return StatusCode((int)ex.StatusCode, new BaseResponse<string>
                {
                    StatusCode = ex.StatusCode,
                    Message = ex.ErrorDetail.ErrorMessage.ToString(),
                    Code = (int)ex.StatusCode
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi xảy ra khi đăng ký tài khoản.");
                return StatusCode(500, new BaseResponse<string>
                {
                    StatusCode = Core.Store.StatusCodes.ServerError,
                    Message = "Có lỗi xảy ra, vui lòng thử lại sau.",
                    Code = (int)Core.Store.StatusCodes.ServerError
                });
            }
        }


        [Authorize(Roles = "Admin")]
        [HttpGet("check-current")]
        public async Task<ActionResult<BaseResponse<string>>> CheckUserCurrent()
        {
            try
            {
                var idUser = _currentUserService.GetUserId();
                return Ok(BaseResponse<string>.OkResponse(idUser, "Successfull"));
            }
            catch(BaseException.ErrorException ex)
            {
                return StatusCode((int)ex.StatusCode, new BaseResponse<string>
                {
                    StatusCode = ex.StatusCode,
                    Message = ex.ErrorDetail.ErrorMessage.ToString(),
                    Code = (int)ex.StatusCode
                });
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
