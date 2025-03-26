using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD392_AffiliLinker.Services.DTO.ClickDTO.Request
{
    public class FillterExportClickInfo
    {
        public string AffiliateId { get; set; }
        public DateTime? BeginDate { get; set; }
        public DateTime? EndDate { get; set; } 
    }
}
