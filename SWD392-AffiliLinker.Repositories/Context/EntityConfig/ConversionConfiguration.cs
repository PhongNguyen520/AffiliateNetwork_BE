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
	public class ConversionConfiguration : IEntityTypeConfiguration<Conversion>
	{
		public void Configure(EntityTypeBuilder<Conversion> builder)
		{
			builder.ToTable("Conversions");

			// Khóa chính
			builder.HasKey(c => c.Id);

			// Cấu hình thuộc tính OrderId, ProductId
			builder.Property(c => c.OrderId)
				.HasMaxLength(255)
				.IsRequired(false); // Có thể null

			builder.Property(c => c.ProductId)
				.HasMaxLength(255)
				.IsRequired(false); // Có thể null

			// Cấu hình Quantity
			builder.Property(c => c.Quantity)
				.IsRequired()
				.HasDefaultValue(1); // Giá trị mặc định là 1

			// Cấu hình Subtotal
			builder.Property(c => c.Subtotal)
				.HasColumnType("decimal(18,2)") // Kiểu số thập phân chính xác
				.IsRequired(false); // Có thể null

			// Cấu hình Commission
			builder.Property(c => c.Commission)
				.HasColumnType("decimal(18,2)")
				.IsRequired();

			// Cấu hình Status
			builder.Property(c => c.Status)
				.HasMaxLength(100)
				.IsRequired();

			// Cấu hình IsPayment
			builder.Property(c => c.IsPayment)
				.IsRequired(false); // Có thể null

			// Quan hệ với AffiliateLink
			builder.HasOne(c => c.AffiliateLink)
				.WithMany(a => a.Conversions)
				.HasForeignKey(c => c.AffiliateLinkId)
				.OnDelete(DeleteBehavior.Cascade); // Xóa AffiliateLink sẽ xóa Conversion
		}
	}


}
