using SWD392_AffiliLinker.Core.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD392_AffiliLinker.Repositories.Entities
{
	public class AffiliateLink : BaseEntity
	{
		public string Url { get; set; } = string.Empty;
		public string UtmSource { get; set; } = string.Empty;
		public string UtmMedium { get; set; } = string.Empty;
		public string UtmCampaign { get; set; } = string.Empty;
		public string UtmContent { get; set; } = string.Empty;
		public string? ShortenUrl { get; set; }
		public string? OptimizeUrl { get; set; }
		public string Status { get; set; } = string.Empty;

		public string CampaignId { get; set; }
		public virtual Campaign Campaign { get; set; } = null!;

		public Guid UserId { get; set; }
		public virtual User User { get; set; } = null!;

		public virtual ICollection<ClickCount> ClickCounts { get; set; } = new List<ClickCount>();
		public virtual ICollection<ClickInfo> ClickInfos { get; set; } = new List<ClickInfo>();
		public virtual ICollection<Conversion> Conversions { get; set; } = new List<Conversion>();
	}

}
