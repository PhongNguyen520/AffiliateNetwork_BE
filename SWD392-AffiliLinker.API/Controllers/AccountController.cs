using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SWD392_AffiliLinker.Core.Base;
using SWD392_AffiliLinker.Core.Utils;
using SWD392_AffiliLinker.Services.DTO.AccountDTO.Request;
using SWD392_AffiliLinker.Services.DTO.AccountDTO.Response;
using SWD392_AffiliLinker.Services.Interfaces;

namespace SWD392_AffiliLinker.API.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAdvertiserById (string id)
        {
            try
            {
                var account = await _accountService.GetAdvertiserUserById(id);

                if (account != null)
                {
                    return Ok(BaseResponse<AdvertiserAccountResponse>.OkResponse(account, "OK"));
                }

                if (account is null)
                {
                    var accountDup = await _accountService.GetPublisherUserById(id);
                    if (accountDup != null)
                    {
                        return Ok(BaseResponse<PublisherAccountResponse>.OkResponse(accountDup, "OK"));
                    }
                }
                throw new BaseException.ErrorException(Core.Store.StatusCodes.NotFound, Core.Store.StatusCodes.NotFound.Name(), "Account not exist!!!");
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
                return StatusCode(500, new BaseResponse<string>
                {
                    StatusCode = Core.Store.StatusCodes.ServerError,
                    Message = ex.Message,
                    Code = (int)Core.Store.StatusCodes.ServerError
                });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAccount ([FromQuery] FillterAccountRequest request)
        {
            try
            {
                var result = await _accountService.GetAll(request);
                return Ok(BaseResponse<BasePaginatedList<AccountResponse>>.OkResponse(result, "OK"));
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
                return StatusCode(500, new BaseResponse<string>
                {
                    StatusCode = Core.Store.StatusCodes.ServerError,
                    Message = ex.Message,
                    Code = (int)Core.Store.StatusCodes.ServerError
                });
            }
        }

        [HttpPut("{id}/avatar")]
        public async Task<IActionResult> UpdateAvater (IFormFile avatar, string id)
        {
            try
            {
                var result = await _accountService.UpdateAvatar(id, avatar);
                return Ok(BaseResponse<string>.OkResponse(result, "OK"));
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
                return StatusCode(500, new BaseResponse<string>
                {
                    StatusCode = Core.Store.StatusCodes.ServerError,
                    Message = ex.Message,
                    Code = (int)Core.Store.StatusCodes.ServerError
                });
            }
        }
    }
}
