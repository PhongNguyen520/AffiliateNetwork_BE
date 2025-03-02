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
	public class ReportMediaConfiguration : IEntityTypeConfiguration<ReportMedia>
	{
		public void Configure(EntityTypeBuilder<ReportMedia> builder)
		{
			// Đặt tên bảng
			builder.ToTable("ReportMedias");

			// Thiết lập khóa chính
			builder.HasKey(rm => rm.FileName); // Giả sử FileName là duy nhất

			// Cấu hình các cột
			builder.Property(rm => rm.FileName)
				.HasMaxLength(255)
				.IsRequired();

			builder.Property(rm => rm.Url)
				.HasMaxLength(1000)
				.IsRequired();

			builder.Property(rm => rm.Type)
				.HasMaxLength(100)
				.IsRequired();

			builder.Property(rm => rm.Size)
				.HasMaxLength(50)
				.IsRequired();

			builder.Property(rm => rm.Status)
				.HasMaxLength(50)
				.IsRequired();
		}
	}
}
