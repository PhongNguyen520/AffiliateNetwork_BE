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
	public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
	{
		public void Configure(EntityTypeBuilder<Notification> builder)
		{
			// Đặt tên bảng
			builder.ToTable("Notifications");

			// Thiết lập khóa chính
			builder.HasKey(n => n.Id);

			// Cấu hình các cột
			builder.Property(n => n.Type)
				.HasMaxLength(100) // Giới hạn độ dài loại thông báo
				.IsRequired();

			builder.Property(n => n.Message)
				.HasMaxLength(1000) // Giới hạn độ dài nội dung thông báo
				.IsRequired();

			builder.Property(n => n.Status)
				.HasMaxLength(50)
				.IsRequired();

			// Cấu hình quan hệ với User
			builder.HasOne(n => n.User)
				.WithMany()
				.HasForeignKey(n => n.UserId)
				.OnDelete(DeleteBehavior.Cascade); // Khi xóa User, xóa luôn Notification liên quan
		}
	}

}
