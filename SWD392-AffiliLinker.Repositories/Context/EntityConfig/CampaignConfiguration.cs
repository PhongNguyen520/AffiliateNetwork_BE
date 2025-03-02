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
	public class CampaignConfiguration : IEntityTypeConfiguration<Campaign>
	{
		public void Configure(EntityTypeBuilder<Campaign> builder)
		{
			builder.ToTable("Campaigns");

			// Khóa chính
			builder.HasKey(c => c.Id);

			// Cấu hình các thuộc tính
			builder.Property(c => c.WebsiteLink)
				.HasMaxLength(500)
				.IsRequired();

			builder.Property(c => c.CampaignName)
				.HasMaxLength(255)
				.IsRequired();

			builder.Property(c => c.Introduction)
				.HasMaxLength(1000);

			builder.Property(c => c.Description)
				.HasMaxLength(4000);

			builder.Property(c => c.Policy)
				.HasMaxLength(2000);

			builder.Property(c => c.Image)
				.HasMaxLength(500);

			builder.Property(c => c.EnrollCount)
				.HasDefaultValue(0);

			builder.Property(c => c.ConversionRate)
				.HasColumnType("decimal(5,2)")
				.HasDefaultValue(0);

			builder.Property(c => c.StartDate)
				.IsRequired();

			builder.Property(c => c.EndDate)
				.IsRequired();

			builder.Property(c => c.TargetCustomer)
				.HasMaxLength(500);

			builder.Property(c => c.Zone)
				.HasMaxLength(255);

			builder.Property(c => c.Status)
				.HasMaxLength(100)
				.IsRequired();

			// Thiết lập quan hệ 1-N với User
			builder.HasOne(c => c.User)
				.WithMany(u => u.Campaigns)
				.HasForeignKey(c => c.UserId)
				.OnDelete(DeleteBehavior.Cascade); // Xóa User thì Campaign cũng bị xóa

			// Thiết lập quan hệ 1-N với CampaignCategory
			builder.HasOne(c => c.Category)
				.WithMany(cc => cc.Campaigns)
				.HasForeignKey(c => c.CategoryId)
				.OnDelete(DeleteBehavior.SetNull); // Khi xóa Category, giữ lại Campaign nhưng CategoryId = NULL

			// Quan hệ N-N với PayoutModel qua bảng trung gian CampaignPayoutModel
			builder.HasMany(c => c.CampaignPayoutModels)
				.WithOne(cp => cp.Campaign)
				.HasForeignKey(cp => cp.CampaignId)
				.OnDelete(DeleteBehavior.Cascade);

			// Quan hệ N-N với User qua CampaignMember
			builder.HasMany(c => c.CampaignMembers)
				.WithOne(cm => cm.Campaign)
				.HasForeignKey(cm => cm.CampaignId)
				.OnDelete(DeleteBehavior.Cascade);
		}
	}

}
