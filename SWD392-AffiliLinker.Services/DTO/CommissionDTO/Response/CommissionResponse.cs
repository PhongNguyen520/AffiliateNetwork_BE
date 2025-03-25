using SWD392_AffiliLinker.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD392_AffiliLinker.Services.DTO.CommissionDTO.Response
{
    public class CommissionResponse
    {
        public string Id { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset LastUpdatedTime { get; set; }
        public decimal TotalCommission { get; set; }
        public decimal? ApprovalCommission { get; set; }
    }
}
