using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SWD392_AffiliLinker.Core.Store.EnumStatus;

namespace SWD392_AffiliLinker.Services.DTO.CampaginDTO.Request
{
    public class CampaignRequest
    {
        public string CampaignName { get; set; }
        public string? Description { get; set; }
        public string? Introduction { get; set; }
        public string? Policy { get; set; }
        public string? Image { get; set; }
        public string? WebsiteLink { get; set; }
        public string? TargetCustomer { get; set; }
        public string? Zone { get; set; }
        public string? CategoryId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
    }

    public class UpdateStatusRequest
    {
        public CampaignStatus Status { get; set; }
    }
}
