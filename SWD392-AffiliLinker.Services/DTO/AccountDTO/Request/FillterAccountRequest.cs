using SWD392_AffiliLinker.Core.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SWD392_AffiliLinker.Core.Store.EnumStatus;

namespace SWD392_AffiliLinker.Services.DTO.AccountDTO.Request
{
    public class FillterAccountRequest
    {
        public UserStatus? Status { get; set; }
        public int? PageIndex { get; set; }
        public int? PageSize { get; set; }
    }
}
