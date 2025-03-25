using SWD392_AffiliLinker.Services.DTO.ClickDTO.Request;
using SWD392_AffiliLinker.Services.DTO.ConversionDTO.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD392_AffiliLinker.Services.Interfaces
{
    public interface IExportTrafficService
    {
        Task<byte[]> ExportClickInfoExcel(FillterExportClickInfo request);
        Task<byte[]> ExportConversionExcel(FillterExportConversion request);
    }
}
