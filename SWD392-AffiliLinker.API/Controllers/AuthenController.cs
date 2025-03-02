using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SWD392_AffiliLinker.Core.Base;
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
		private readonly IUnitOfWork _unitOfWork;

		public AuthController(IAuthenticationService authenticationService, IUnitOfWork unitOfWork, ILogger<AuthController> logger)
		{
			_authenticationService = authenticationService;
			_unitOfWork = unitOfWork;
			_logger = logger;
		}

		[AllowAnonymous]
		[HttpPost("register")]
		public async Task<ActionResult<BaseResponse<string>>> Register(RegisterRequest registerModelView)
		{
			try
			{
				_unitOfWork.BeginTransaction();
				var result = await _authenticationService.RegisterAsync(registerModelView);
				if (result.StatusCode == Core.Store.StatusCodes.BadRequest)
				{
					_unitOfWork.RollBack();
					return BadRequest(result);
				}
				_unitOfWork.CommitTransaction();
				return Ok(result);
			}
			catch (Exception ex)
			{
				_unitOfWork.RollBack();
				_logger.LogError(ex, "Lỗi xảy ra khi đăng ký tài khoản.");
				return StatusCode(500, new BaseResponse<string>
				{
					StatusCode = Core.Store.StatusCodes.ServerError,
					Message = "Có lỗi xảy ra, vui lòng thử lại sau."
				});
			}

		}

		[HttpPost("login")]
		public async Task<ActionResult<BaseResponse<string>>> Login(LoginRequest model)
		{
			try
			{
				_unitOfWork.BeginTransaction();
				AuthenResponse? result = await _authenticationService.Login(model);
				string mess = "Login successful";
				if (result == null)
				{
					mess = "Login fail";
					return Ok(BaseResponse<AuthenResponse>.OkResponse(result, mess));
				}
				_unitOfWork.CommitTransaction();
				return Ok(result);
			}
			catch (Exception ex)
			{
				_unitOfWork.RollBack();
				_logger.LogError(ex, "Lỗi xảy ra khi đăng ký tài khoản.");
				return StatusCode(500, new BaseResponse<string>
				{
					StatusCode = Core.Store.StatusCodes.ServerError,
					Message = "Có lỗi xảy ra, vui lòng thử lại sau."
				});
			}
		}
	}
}
