using SWD392_AffiliLinker.Core.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD392_AffiliLinker.Repositories.Entities
{
	public class AffiliateLink : BaseEntity
	{
		public string Url { get; set; }
		public string? UtmSource { get; set; }
		public string? UtmMedium { get; set; }
		public string? UtmCampaign { get; set; }
		public string? UtmContent { get; set; }
		public string? ShortenUrl { get; set; }
		public string? OptimizeUrl { get; set; }
		public string Status { get; set; }

		public string CampaignId { get; set; }
		public virtual Campaign Campaign { get; set; }

		public Guid UserId { get; set; }
		public virtual User User { get; set; } = null!;

		public virtual ICollection<ClickCount> ClickCounts { get; set; } = new List<ClickCount>();
		public virtual ICollection<ClickInfo> ClickInfos { get; set; } = new List<ClickInfo>();
		public virtual ICollection<Conversion> Conversions { get; set; } = new List<Conversion>();
	}

}
