using Microsoft.AspNetCore.Mvc;
using SWD392_AffiliLinker.Core.Base;
using SWD392_AffiliLinker.Core.Store;
using SWD392_AffiliLinker.Core.Utils;
using SWD392_AffiliLinker.Repositories.Entities;
using SWD392_AffiliLinker.Services.DTO.ClickDTO.Request;
using SWD392_AffiliLinker.Services.DTO.ClickDTO.Response;
using SWD392_AffiliLinker.Services.DTO.ConversionDTO.Request;
using SWD392_AffiliLinker.Services.DTO.UserCurrentDTO.Response;
using SWD392_AffiliLinker.Services.Interfaces;
using static SWD392_AffiliLinker.Core.Store.EnumStatus;

namespace SWD392_AffiliLinker.API.Controllers
{
    public class TrackingLinkController : Controller
    {
        private readonly IAffiliateLinkService _affiliateLinkService;
        private readonly IClickService _clickService;
        private readonly ICookieService _cookieService;
        private readonly ICurrentUserService _currentUserService;

        public TrackingLinkController(IAffiliateLinkService affiliateLinkService, IClickService clickService, ICookieService cookieService, ICurrentUserService currentUserService)
        {
            _affiliateLinkService = affiliateLinkService;
            _clickService = clickService;
            _cookieService = cookieService;
            _currentUserService = currentUserService;
        }

        [HttpGet("o/{slug}")]
        public async Task<IActionResult> RedirectOptimizeUrl(string? slug)
        {
            try
            {
                var affiLink = await _affiliateLinkService.RedirectOptimizeUrl(slug);
                var cookieValue = _cookieService.GetCookie(".Affiliate.Tracking.Application");
                InfoClientResponse infoClientResponse = await _currentUserService.GetClientInfo();

                ClickInfoRequest clickInfoRequest = new()
                {
                    IPAddress = infoClientResponse.IpAddress,
                    UserAgent = infoClientResponse.UserAgent,
                    Status = ClickInfoStatus.Invalid.Name(),
                    AffiliateLinkId = affiLink.Id,
                };

                if (cookieValue != affiLink.Id)
                {
                    _cookieService.SetCookie(affiLink.Id);
                    clickInfoRequest.Status = ClickInfoStatus.Valid.Name();
                }

                await _clickService.CreateClickInfo(clickInfoRequest);

                return Redirect(affiLink.Url);
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

        [HttpGet("s/{shortenCode}")]
        public async Task<IActionResult> RedirectShortenUrl(string? shortenCode)
        {
            try
            {
                var affiLink = await _affiliateLinkService.RedirectShortenUrl(shortenCode);
                var cookieValue = _cookieService.GetCookie(".Affiliate.Tracking.Application");
                InfoClientResponse infoClientResponse = await _currentUserService.GetClientInfo();

                ClickInfoRequest clickInfoRequest = new()
                {
                    IPAddress = infoClientResponse.IpAddress,
                    UserAgent = infoClientResponse.UserAgent,
                    Status = ClickInfoStatus.Invalid.Name(),
                    AffiliateLinkId = affiLink.Id,
                };

                if (cookieValue != affiLink.Id)
                {
                    _cookieService.SetCookie(affiLink.Id);
                    clickInfoRequest.Status = ClickInfoStatus.Valid.Name();
                }

                await _clickService.CreateClickInfo(clickInfoRequest);

                return Redirect($"{affiLink.Url}?affiliateId={affiLink.Id}");
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

        [HttpGet("total-click")]
        public async Task<ActionResult<BaseResponse<BasePaginatedList<TotalClickInfoResponse>>>> ListTotalClickInfo (TotalClickInfoRequest request)
        {
            try
            {
                var response = await _clickService.GetTotalClickInfo(request);
                return Ok(BaseResponse<BasePaginatedList<TotalClickInfoResponse>>.OkResponse(response, Core.Store.StatusCodes.OK.Name()));
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
