using SWD392_AffiliLinker.Core.Base;

namespace SWD392_AffiliLinker.Repositories.Entities
{
	public class Campaign : BaseEntity
	{
		public string WebsiteLink { get; set; } = string.Empty;
		public string CampaignName { get; set; } = string.Empty;
		public string? Introduction { get; set; }
		public string? Description { get; set; }
		public string? Policy { get; set; }
		public string? Image { get; set; }
		public int EnrollCount { get; set; }
		public decimal ConversionRate { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public string? TargetCustomer { get; set; }
		public string? Zone { get; set; }
		public string Status { get; set; } = string.Empty;

		public Guid UserId { get; set; }
		public virtual User User { get; set; } = null!;

		public string? CategoryId { get; set; }
		public virtual CampaignCategory? Category { get; set; } = null!;

		public virtual ICollection<CampaignMember> CampaignMembers { get; set; } = new List<CampaignMember>();
		public virtual ICollection<CampaignPayoutModel> CampaignPayoutModels { get; set; } = new List<CampaignPayoutModel>();

		// Thêm quan hệ ngược với AffiliateLink
		public virtual ICollection<AffiliateLink> AffiliateLinks { get; set; } = new List<AffiliateLink>();
	}

}
