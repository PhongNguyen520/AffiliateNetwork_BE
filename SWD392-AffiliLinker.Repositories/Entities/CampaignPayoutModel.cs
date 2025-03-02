using SWD392_AffiliLinker.Core.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD392_AffiliLinker.Repositories.Entities
{
	public class CampaignPayoutModel
	{
		public string CampaignId { get; set; }
		public virtual Campaign Campaign { get; set; } = null!;

		public string PayoutModelId { get; set; }
		public virtual PayoutModel PayoutModel { get; set; } = null!;

		public string Status { get; set; } = string.Empty;

		public DateTimeOffset CreatedTime { get; set; } = DateTimeOffset.UtcNow;
		public DateTimeOffset LastUpdatedTime { get; set; } = DateTimeOffset.UtcNow;
		public DateTimeOffset? DeletedTime { get; set; }
	}

}
