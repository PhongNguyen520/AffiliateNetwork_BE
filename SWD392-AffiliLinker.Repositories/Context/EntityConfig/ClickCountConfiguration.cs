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
	public class ClickCountConfiguration : IEntityTypeConfiguration<ClickCount>
	{
		public void Configure(EntityTypeBuilder<ClickCount> builder)
		{
			// Đặt tên bảng
			builder.ToTable("ClickCounts");

			// Thiết lập khóa chính
			builder.HasKey(cc => cc.Id);

			// Cấu hình cột Count
			builder.Property(cc => cc.Count)
				.IsRequired(); // Không cho phép null

			// Thiết lập quan hệ với AffiliateLink
			builder.HasOne(cc => cc.AffiliateLink)
				.WithMany(al => al.ClickCounts) // Một AffiliateLink có nhiều ClickCounts
				.HasForeignKey(cc => cc.AffiliateLinkId)
				.OnDelete(DeleteBehavior.Cascade); // Xóa AffiliateLink thì xóa luôn ClickCounts
		}
	}

}
