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
	public class AffiliateLinkConfiguration : IEntityTypeConfiguration<AffiliateLink>
	{
		public void Configure(EntityTypeBuilder<AffiliateLink> builder)
		{
			builder.ToTable("AffiliateLinks");

			// Khóa chính
			builder.HasKey(al => al.Id);

			// Cấu hình các thuộc tính
			builder.Property(al => al.Url)
				.HasMaxLength(500)
				.IsRequired();

			builder.Property(al => al.UtmSource)
				.HasMaxLength(255)
				.IsRequired(false);

			builder.Property(al => al.UtmMedium)
				.HasMaxLength(255)
				.IsRequired(false);

			builder.Property(al => al.UtmCampaign)
				.HasMaxLength(255)
				.IsRequired(false);

			builder.Property(al => al.UtmContent)
				.HasMaxLength(255) // Không bắt buộc
			    .IsRequired(false);

            builder.Property(al => al.ShortenUrl)
				.HasMaxLength(500)
                .IsRequired(false);

            builder.Property(al => al.OptimizeUrl)
				.HasMaxLength(500)
                .IsRequired(false);

            builder.Property(al => al.Status)
				.HasMaxLength(100)
				.IsRequired();

			// Thêm Index tối ưu truy vấn
			builder.HasIndex(al => al.CampaignId);
			builder.HasIndex(al => al.UserId);

			// Thiết lập quan hệ 1-N với Campaign
			builder.HasOne(al => al.Campaign)
				.WithMany(c => c.AffiliateLinks)
				.HasForeignKey(al => al.CampaignId)
				.OnDelete(DeleteBehavior.NoAction); // Tránh lỗi xóa nhầm

			// Thiết lập quan hệ 1-N với User
			builder.HasOne(al => al.User)
				.WithMany(u => u.AffiliateLinks)
				.HasForeignKey(al => al.UserId)
				.OnDelete(DeleteBehavior.NoAction);

			// Quan hệ 1-N với ClickCount
			builder.HasMany(al => al.ClickCounts)
				.WithOne(cc => cc.AffiliateLink)
				.HasForeignKey(cc => cc.AffiliateLinkId)
				.OnDelete(DeleteBehavior.Cascade);

			// Quan hệ 1-N với ClickInfo
			builder.HasMany(al => al.ClickInfos)
				.WithOne(ci => ci.AffiliateLink)
				.HasForeignKey(ci => ci.AffiliateLinkId)
				.OnDelete(DeleteBehavior.Cascade);

			// Quan hệ 1-N với Conversion
			builder.HasMany(al => al.Conversions)
				.WithOne(conv => conv.AffiliateLink)
				.HasForeignKey(conv => conv.AffiliateLinkId)
				.OnDelete(DeleteBehavior.Cascade);
		}
	}

}
