using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SWD392_AffiliLinker.Core.Store.EnumStatus;

namespace SWD392_AffiliLinker.Services.DTO.ConversionDTO.Request
{
    public class FillterConverByAffiLinkId
    {
        public string AffiLinkId { get; set; }
        public ConversionStatus? Status { get; set; }
        public bool? IsPayment {  get; set; }
        public DateTime? BeginDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? PageIndex { get; set; }
        public int? PageSize { get; set; }
    }
}
