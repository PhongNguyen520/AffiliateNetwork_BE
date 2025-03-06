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
	public class PublisherConfiguration : IEntityTypeConfiguration<Publisher>
	{
		public void Configure(EntityTypeBuilder<Publisher> builder)
		{
			builder.ToTable("Publishers");

			// Khóa chính
			builder.HasKey(p => p.Id);

			// Cấu hình thuộc tính TaxCode
			builder.Property(p => p.TaxCode)
				.HasMaxLength(50)
				.IsRequired(false);

			// Cấu hình quan hệ 1-1 với User
			builder.HasOne(p => p.User)
				.WithOne()
				.HasForeignKey<Publisher>(p => p.UserId)
				.OnDelete(DeleteBehavior.Cascade); // Khi xóa User, Publisher cũng bị xóa

			// Cấu hình quan hệ 1-N với Commission
			builder.HasMany(p => p.Commissions)
				.WithOne(c => c.Publisher)
				.HasForeignKey(c => c.PublisherId)
				.OnDelete(DeleteBehavior.Restrict); // Không xóa Commission khi xóa Publisher
		}
	}

}
