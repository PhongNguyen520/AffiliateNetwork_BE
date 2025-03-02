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
	public class ReportConfiguration : IEntityTypeConfiguration<Report>
	{
		public void Configure(EntityTypeBuilder<Report> builder)
		{
			// Đặt tên bảng
			builder.ToTable("Reports");

			// Thiết lập khóa chính
			builder.HasKey(r => r.Id);

			// Cấu hình các cột
			builder.Property(r => r.Title)
				.HasMaxLength(255) // Giới hạn độ dài tiêu đề
				.IsRequired();

			builder.Property(r => r.Content)
				.HasMaxLength(2000) // Giới hạn độ dài nội dung báo cáo
				.IsRequired();

			builder.Property(r => r.Status)
				.HasMaxLength(50) // Giới hạn độ dài trạng thái
				.IsRequired();

			builder.Property(r => r.RelyForReportId)
				.IsRequired(false); // Cho phép null nếu không phải báo cáo phản hồi

			builder.HasOne(f => f.User)
				.WithMany()
				.HasForeignKey(f => f.UserId)
				.OnDelete(DeleteBehavior.NoAction);

			builder.HasOne(f => f.Campaign)
				.WithMany()
				.HasForeignKey(f => f.CampaignId)
				.OnDelete(DeleteBehavior.Cascade);
		}
	}

}
