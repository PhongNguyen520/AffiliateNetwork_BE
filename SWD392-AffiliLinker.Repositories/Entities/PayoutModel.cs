using SWD392_AffiliLinker.Core.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD392_AffiliLinker.Repositories.Entities
{
	public class PayoutModel : BaseEntity
	{
		public string Name { get; set; }
		public string? Description { get; set; }
		public string? Status { get; set; }
		public virtual ICollection<CampaignPayoutModel> CampaignPayoutModels { get; set; } = new List<CampaignPayoutModel>();
	}
}
