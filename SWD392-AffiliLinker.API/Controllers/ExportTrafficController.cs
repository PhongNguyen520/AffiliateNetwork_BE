using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using SWD392_AffiliLinker.Core.Base;
using SWD392_AffiliLinker.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SWD392_AffiliLinker.API.Controllers
{
    [Route("export-traffic")]
    [ApiController]
    public class ExportTrafficController : ControllerBase
    {
        private readonly IExportTrafficService _exportTraffic;

        public ExportTrafficController(IExportTrafficService exportTrafficService)
        {
            _exportTraffic = exportTrafficService;
        }

        [HttpGet("export-excel/clickinfo")]
        public async Task<IActionResult> ExportClickInfoExcel(string AffiLinkId)
        {
            try
            {
                var fileBytes = await _exportTraffic.ExportClickInfoExcel(AffiLinkId);
                return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ClickInFo.xlsx");
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

        [HttpGet("export-excel/conversion")]
        public async Task<IActionResult> ExportConversionExcel(string AffiLinkId)
        {
            try
            {
                var fileBytes = await _exportTraffic.ExportConversionExcel(AffiLinkId);
                return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Conversion.xlsx");
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
