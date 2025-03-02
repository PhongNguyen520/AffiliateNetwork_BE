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
	public class CommissionConfiguration : IEntityTypeConfiguration<Commission>
	{
		public void Configure(EntityTypeBuilder<Commission> builder)
		{
			builder.ToTable("Commissions");

			// Khóa chính
			builder.HasKey(c => c.Id);

			// Cấu hình TotalCommission
			builder.Property(c => c.TotalCommission)
				.HasColumnType("decimal(18,2)")
				.IsRequired();

			// Cấu hình ApprovalCommission (có thể null)
			builder.Property(c => c.ApprovalCommission)
				.HasColumnType("decimal(18,2)")
				.IsRequired(false);

			// Cấu hình PublisherId
			builder.Property(c => c.PublisherId)
				.IsRequired();

			// Quan hệ với Publisher
			builder.HasOne(c => c.Publisher)
				.WithMany(p => p.Commissions) // Một Publisher có nhiều Commission
				.HasForeignKey(c => c.PublisherId)
				.OnDelete(DeleteBehavior.Cascade); // Xóa Publisher sẽ xóa tất cả Commission liên quan
		}
	}


}
