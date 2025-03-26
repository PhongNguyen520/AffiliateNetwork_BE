using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWD392_AffiliLinker.Core.Base;
using SWD392_AffiliLinker.Services.DTO.ConversionDTO.Request;
using SWD392_AffiliLinker.Services.DTO.ConversionDTO.Response;
using SWD392_AffiliLinker.Services.Interfaces;
using SWD392_AffiliLinker.Services.Services;

namespace SWD392_AffiliLinker.API.Controllers
{
    [Route("api/conversion")]
    [ApiController]
    public class ConversionController : ControllerBase
    {
        private readonly IConversionService _conversionService;

        public ConversionController(IConversionService conversionService)
        {
            _conversionService = conversionService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateConversion([FromBody] CreateConversion request)
        {
            try
            {
                var result = await _conversionService.CreateConversionAsync(request);
                return Ok(BaseResponse<string>.OkResponse(result, "Successfull"));
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetConversionById(string id)
        {
            try
            {
                var result = await _conversionService.GetConversionById(id);
                return Ok(BaseResponse<ConversionResponse>.OkResponse(result, "Successfull"));
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

        [HttpGet("fillter")]
        public async Task<IActionResult> FillterListConversion([FromQuery] FillterConverByAffiLinkId request)
        {
            try
            {
                var result = await _conversionService.ListConverByAffiLinkId(request);
                return Ok(BaseResponse<BasePaginatedList<ConversionResponse>>.OkResponse(result, "Successfull"));
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
