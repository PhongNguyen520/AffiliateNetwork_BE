using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD392_AffiliLinker.Services.Interfaces
{
    public interface IExportTrafficService
    {
        Task<byte[]> ExportClickInfoExcel(string AffiLinkId);
        Task<byte[]> ExportConversionExcel(string AffiLinkId);
    }
}
