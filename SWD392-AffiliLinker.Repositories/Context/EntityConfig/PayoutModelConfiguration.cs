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
	public class PayoutModelConfiguration : IEntityTypeConfiguration<PayoutModel>
	{
		public void Configure(EntityTypeBuilder<PayoutModel> builder)
		{
			builder.ToTable("PayoutModels");

			// Khóa chính
			builder.HasKey(p => p.Id);

			// Cấu hình thuộc tính
			builder.Property(p => p.Name)
				.HasMaxLength(255)
				.IsRequired();

			builder.Property(p => p.Description)
				.HasMaxLength(500)
				.IsRequired(false); // Nullable

			builder.Property(p => p.Status)
				.HasMaxLength(100)
				.IsRequired(false); // Nullable

			// Cấu hình quan hệ 1-N với Campaign
			builder.HasMany(p => p.CampaignPayoutModels)
				.WithOne(c => c.PayoutModel)
				.HasForeignKey(c => c.PayoutModelId)
				.OnDelete(DeleteBehavior.Restrict); // Không xóa campaign khi xóa payout model
		}
	}


}
