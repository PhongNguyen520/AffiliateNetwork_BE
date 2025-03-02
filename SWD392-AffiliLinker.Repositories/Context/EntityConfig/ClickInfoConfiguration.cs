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
	public class ClickInfoConfiguration : IEntityTypeConfiguration<ClickInfo>
	{
		public void Configure(EntityTypeBuilder<ClickInfo> builder)
		{
			builder.ToTable("ClickInfos");

			// Khóa chính
			builder.HasKey(c => c.Id);

			// Cấu hình IPAddress
			builder.Property(c => c.IPAddress)
				.HasMaxLength(45) // Hỗ trợ IPv4 và IPv6
				.IsRequired(false);

			// Cấu hình UserAgent
			builder.Property(c => c.UserAgent)
				.HasMaxLength(500) // Giới hạn độ dài UserAgent
				.IsRequired(false);

			// Cấu hình Status
			builder.Property(c => c.Status)
				.HasMaxLength(100)
				.IsRequired(false);

			// Cấu hình AffiliateLinkId
			builder.Property(c => c.AffiliateLinkId)
				.IsRequired();

			// Quan hệ với AffiliateLink
			builder.HasOne(c => c.AffiliateLink)
				.WithMany(a => a.ClickInfos) // Một AffiliateLink có nhiều ClickInfo
				.HasForeignKey(c => c.AffiliateLinkId)
				.OnDelete(DeleteBehavior.Cascade); // Khi xóa AffiliateLink thì ClickInfo cũng bị xóa
		}
	}


}
