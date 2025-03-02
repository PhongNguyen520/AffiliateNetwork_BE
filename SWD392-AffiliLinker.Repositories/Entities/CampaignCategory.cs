using SWD392_AffiliLinker.Core.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD392_AffiliLinker.Repositories.Entities
{
	public class CampaignCategory : BaseEntity
	{
		public string? Name { get; set; }
		public string? Status { get; set; }
		public virtual ICollection<Campaign> Campaigns { get; set;} = new List<Campaign>();
	}
}
