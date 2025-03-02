using SWD392_AffiliLinker.Core.Base;

namespace SWD392_AffiliLinker.Repositories.Entities
{
	public class Feedback : BaseEntity
	{
		public int Rate { get; set; }
		public string Content { get; set; }
		public string Status { get; set; }
		public string CampaignId { get; set; }
		public virtual Campaign Campaign { get; set; }
		public Guid UserId { get; set; }
		public virtual User User { get; set; }
	}
}
