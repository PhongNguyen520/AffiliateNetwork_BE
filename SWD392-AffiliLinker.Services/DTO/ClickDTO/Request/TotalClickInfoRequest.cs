using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static SWD392_AffiliLinker.Core.Store.EnumStatus;

namespace SWD392_AffiliLinker.Services.DTO.ClickDTO.Request
{
    public class TotalClickInfoRequest
    {
        public string AffiLinkId { get; set; }

        public ClickInfoStatus? Status { get; set; }

        public DateTime? BeginDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int? Index { get; set; }

        public int? Size { get; set; }
    }
}
