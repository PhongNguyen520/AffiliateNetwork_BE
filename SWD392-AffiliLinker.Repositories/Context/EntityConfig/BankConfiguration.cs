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
	public class BankConfiguration : IEntityTypeConfiguration<Bank>
	{
		public void Configure(EntityTypeBuilder<Bank> builder)
		{
			// Đặt tên bảng
			builder.ToTable("Banks");

			// Khóa chính
			builder.HasKey(b => b.Id);

			// Cấu hình các cột
			builder.Property(b => b.AccountName)
				.HasMaxLength(255)
				.IsRequired();

			builder.Property(b => b.BankName)
				.HasMaxLength(255)
				.IsRequired();

			builder.Property(b => b.BankCode)
				.HasMaxLength(50)
				.IsRequired();

			builder.Property(b => b.BankBranch)
				.HasMaxLength(255)
				.IsRequired();

			builder.Property(b => b.Status)
				.IsRequired();

			// Cấu hình quan hệ với User
			builder.HasOne(b => b.User)
				.WithMany()
				.HasForeignKey(b => b.UserId)
				.OnDelete(DeleteBehavior.Cascade);
		}
	}

}
