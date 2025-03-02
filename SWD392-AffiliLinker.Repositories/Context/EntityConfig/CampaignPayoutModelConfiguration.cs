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
	public class CampaignPayoutModelConfiguration : IEntityTypeConfiguration<CampaignPayoutModel>
	{
		public void Configure(EntityTypeBuilder<CampaignPayoutModel> builder)
		{
			builder.ToTable("CampaignPayoutModels");

			// Thiết lập khóa chính kép
			builder.HasKey(cpm => new { cpm.CampaignId, cpm.PayoutModelId });

			// Cấu hình quan hệ với Campaign
			builder.HasOne(cpm => cpm.Campaign)
				.WithMany(c => c.CampaignPayoutModels)
				.HasForeignKey(cpm => cpm.CampaignId)
				.OnDelete(DeleteBehavior.Cascade);

			// Cấu hình quan hệ với PayoutModel
			builder.HasOne(cpm => cpm.PayoutModel)
				.WithMany()
				.HasForeignKey(cpm => cpm.PayoutModelId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.Property(cpm => cpm.Status)
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
