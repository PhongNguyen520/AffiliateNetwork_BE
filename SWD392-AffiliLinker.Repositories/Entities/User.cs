using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace SWD392_AffiliLinker.Repositories.Entities
{
    public class User : IdentityUser<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DOB { get; set; }
        public string? Avatar { get; set; }
        public string Status { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
        public virtual ICollection<Notification> Notification { get; set; }
        public virtual ICollection<Bank> Banks { get; set; }
        public virtual ICollection<Campaign> Campaigns { get; set; }
        public virtual ICollection<CampaignMember> CampaignMembers { get; set; }
        public virtual ICollection<AffiliateLink> AffiliateLinks { get; set; }
        public virtual Advertiser Advertiser { get; set; }
        public virtual Publisher Publisher { get; set; }
		public string? LastUpdatedBy { get; set; }
		public string? DeletedBy { get; set; }
		public DateTimeOffset CreatedTime { get; set; }
		public DateTimeOffset LastUpdatedTime { get; set; }
		public DateTimeOffset? DeletedTime { get; set; }
	}
}
