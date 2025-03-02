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
	public class CampaignCategoryConfiguration : IEntityTypeConfiguration<CampaignCategory>
	{
		public void Configure(EntityTypeBuilder<CampaignCategory> builder)
		{
			builder.ToTable("CampaignCategories");

			// Định nghĩa khóa chính
			builder.HasKey(cc => cc.Id);

			// Cấu hình Name
			builder.Property(cc => cc.Name)
				.HasMaxLength(255) // Giới hạn độ dài Name
				.IsRequired(false); // Có thể null

			// Cấu hình Status
			builder.Property(cc => cc.Status)
				.HasMaxLength(100)
				.IsRequired(false);

			// Thiết lập quan hệ 1-N với Campaign
			builder.HasMany(cc => cc.Campaigns)
				.WithOne(c => c.Category) // Một Campaign chỉ thuộc một CampaignCategory
				.HasForeignKey(c => c.CategoryId) // Khóa ngoại trong bảng Campaign
				.OnDelete(DeleteBehavior.SetNull); // Khi xóa CampaignCategory, giữ nguyên Campaign nhưng để null CampaignCategoryId
		}
	}


}
