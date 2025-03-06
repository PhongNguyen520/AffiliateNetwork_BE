using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SWD392_AffiliLinker.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace SWD392_AffiliLinker.Repositories.Context.EntityConfig
{
	public class AdvertiserConfiguration : IEntityTypeConfiguration<Advertiser>
	{
		public void Configure(EntityTypeBuilder<Advertiser> builder)
		{

			builder.ToTable("Advertisers");

			// Khóa chính
			builder.HasKey(a => a.Id);

			// Cấu hình cột
			builder.Property(a => a.CampanyName)
				.HasMaxLength(255)
				.IsRequired();

			builder.Property(a => a.CompanyAddress)
				.HasMaxLength(500)
				.IsRequired();

			builder.Property(a => a.Website)
				.HasMaxLength(255)
				.IsRequired();

			builder.Property(a => a.Since)
				.IsRequired();

			builder.Property(a => a.BussinessLicense)
				.HasMaxLength(255)
				.IsRequired();

			// Cấu hình quan hệ với User
			builder.HasOne(a => a.User)
				.WithMany()
				.HasForeignKey(a => a.UserId)
				.OnDelete(DeleteBehavior.Cascade); // Khi xóa User thì Advertiser cũng bị xóa
        }
	}
}
