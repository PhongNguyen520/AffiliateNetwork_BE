using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SWD392_AffiliLinker.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD392_AffiliLinker.Repositories.Context.EntityConfig
{
	public class CampaignMemberConfiguration : IEntityTypeConfiguration<CampaignMember>
	{
		public void Configure(EntityTypeBuilder<CampaignMember> builder)
		{
			builder.ToTable("CampaignMembers");

			// Thiết lập khóa chính kép
			builder.HasKey(cm => new { cm.CampaignId, cm.UserId });

			// Cấu hình quan hệ với Campaign
			builder.HasOne(cm => cm.Campaign)
				.WithMany(c => c.CampaignMembers)
				.HasForeignKey(cm => cm.CampaignId)
				.OnDelete(DeleteBehavior.Cascade);

			// Cấu hình quan hệ với User
			builder.HasOne(cm => cm.User)
				.WithMany()
				.HasForeignKey(cm => cm.UserId)
				.OnDelete(DeleteBehavior.NoAction);

			builder.Property(cm => cm.Status)
				.HasMaxLength(100)
				.IsRequired();

			builder.Property(u => u.CreatedTime)
				.IsRequired();
			builder.Property(u => u.LastUpdatedTime)
				.IsRequired();
			builder.Property(u => u.DeletedTime)
				.IsRequired(false);
		}
	}
}
