using SWD392_AffiliLinker.Core.Base;
using SWD392_AffiliLinker.Services.DTO.ConversionDTO.Request;
using SWD392_AffiliLinker.Services.DTO.ConversionDTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD392_AffiliLinker.Services.Interfaces
{
    public interface IConversionService
    {
        Task<string> CreateConversionAsync (CreateConversion request);
        Task<ConversionResponse> GetConversionById(string converId);
        Task<BasePaginatedList<ConversionResponse>> ListConverByAffiLinkId(FillterConverByAffiLinkId request);
    }
}
