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
	public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
	{
		public void Configure(EntityTypeBuilder<Transaction> builder)
		{
			// Đặt tên bảng
			builder.ToTable("Transactions");

			// Thiết lập khóa chính
			builder.HasKey(t => t.Id);

			// Cấu hình các cột
			builder.Property(t => t.Amount)
				.HasColumnType("decimal(18,2)") // Định dạng số tiền với 2 chữ số thập phân
				.IsRequired();

			builder.Property(t => t.Transaction_Type)
				.HasMaxLength(100)
				.IsRequired();

			builder.Property(t => t.Description)
				.HasMaxLength(500)
				.IsRequired(false); // Cho phép null nếu không có mô tả

			builder.Property(t => t.Status)
				.HasMaxLength(50)
				.IsRequired();

			builder.Property(t => t.UserId)
				.IsRequired();

			// Cấu hình quan hệ với User
			builder.HasOne(t => t.User)
				.WithMany()
				.HasForeignKey(t => t.UserId)
				.OnDelete(DeleteBehavior.Cascade); // Khi xóa User, xóa luôn Transaction liên quan
		}
	}

}
