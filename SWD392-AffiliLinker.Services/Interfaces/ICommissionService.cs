using SWD392_AffiliLinker.Services.DTO.CommissionDTO.Request;
using SWD392_AffiliLinker.Services.DTO.CommissionDTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD392_AffiliLinker.Services.Interfaces
{
    public interface ICommissionService
    {
        Task<List<CommissionResponse>> GetListCommission(FillterCommission request);
    }
}
