using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD392_AffiliLinker.Services.DTO.PaymentDTO.Request
{
    public class VnPaymentRequestModel
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public decimal Amount { get; set; }
        public string Transaction_Type { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }

        public string UserId { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class PayCampaignRequestModel
    {
        public string CampaignId { get; set; }
    }
}
