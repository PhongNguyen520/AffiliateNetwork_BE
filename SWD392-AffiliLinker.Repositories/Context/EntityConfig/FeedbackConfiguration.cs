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
	public class FeedbackConfiguration : IEntityTypeConfiguration<Feedback>
	{
		public void Configure(EntityTypeBuilder<Feedback> builder)
		{
			// Đặt tên bảng
			builder.ToTable("Feedbacks");

			// Thiết lập khóa chính
			builder.HasKey(f => f.Id);

			// Cấu hình các cột
			builder.Property(f => f.Rate)
				.IsRequired();

			builder.Property(f => f.Content)
				.HasMaxLength(1000) // Giới hạn độ dài nội dung phản hồi
				.IsRequired();

			builder.Property(f => f.Status)
				.HasMaxLength(50)
				.IsRequired();

			// Cấu hình quan hệ với Campaign
			builder.HasOne(f => f.Campaign)
				.WithMany()
				.HasForeignKey(f => f.CampaignId)
				.OnDelete(DeleteBehavior.Cascade); // Khi xóa Campaign, xóa luôn Feedback liên quan

			// Cấu hình quan hệ với User
			builder.HasOne(f => f.User)
				.WithMany()
				.HasForeignKey(f => f.UserId)
				.OnDelete(DeleteBehavior.NoAction); // Khi xóa User, xóa luôn Feedback liên quan
		}
	}

}
