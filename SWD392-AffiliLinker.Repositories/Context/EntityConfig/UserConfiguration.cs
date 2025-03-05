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
	public class UserConfiguration : IEntityTypeConfiguration<User>
	{
		public void Configure(EntityTypeBuilder<User> builder)
		{
			builder.ToTable("Users");

			// Định nghĩa khóa chính
			builder.HasKey(u => u.Id);

			// Cấu hình các thuộc tính cơ bản
			builder.Property(u => u.FirstName)
				.HasMaxLength(100)
				.IsRequired();

			builder.Property(u => u.LastName)
				.HasMaxLength(100)
				.IsRequired();

			builder.Property(u => u.Email)
				.HasMaxLength(255)
				.IsRequired();

			builder.Property(u => u.PhoneNumber)
				.HasMaxLength(20);

			builder.Property(u => u.Avatar)
				.HasMaxLength(500);

			builder.Property(u => u.Status)
				.HasMaxLength(50)
				.IsRequired();

			builder.Property(u => u.DOB)
				.IsRequired();

			// Cấu hình trường Audit
			builder.Property(u => u.LastUpdatedBy)
				.HasMaxLength(255);
			builder.Property(u => u.DeletedBy)
				.HasMaxLength(255);
			builder.Property(u => u.CreatedTime)
				.IsRequired();
			builder.Property(u => u.LastUpdatedTime)
				.IsRequired();
			builder.Property(u => u.DeletedTime)
				.IsRequired(false);

			// Cấu hình quan hệ 1-N với Transactions
			builder.HasMany(u => u.Transactions)
				.WithOne(t => t.User)
				.HasForeignKey(t => t.UserId)
				.OnDelete(DeleteBehavior.Cascade);

			// Cấu hình quan hệ 1-N với Notifications
			builder.HasMany(u => u.Notification)
				.WithOne(n => n.User)
				.HasForeignKey(n => n.UserId)
				.OnDelete(DeleteBehavior.Cascade);

			// Cấu hình quan hệ 1-N với Banks
			builder.HasMany(u => u.Banks)
				.WithOne(b => b.User)
				.HasForeignKey(b => b.UserId)
				.OnDelete(DeleteBehavior.Cascade);

			// Cấu hình quan hệ 1-N với Campaigns
			builder.HasMany(u => u.Campaigns)
				.WithOne(c => c.User)
				.HasForeignKey(c => c.UserId)
				.OnDelete(DeleteBehavior.Cascade);

			// Cấu hình quan hệ 1-N với CampaignMembers
			builder.HasMany(u => u.CampaignMembers)
				.WithOne(cm => cm.User)
				.HasForeignKey(cm => cm.UserId)
				.OnDelete(DeleteBehavior.NoAction);

			// Cấu hình quan hệ 1-N với AffiliateLinks
			builder.HasMany(u => u.AffiliateLinks)
				.WithOne(al => al.User)
				.HasForeignKey(al => al.UserId)
				.OnDelete(DeleteBehavior.NoAction);

			// Cấu hình quan hệ 1-1 với Advertiser
			builder.HasOne(u => u.Advertiser)
				.WithOne(a => a.User)
				.HasForeignKey<Advertiser>(a => a.UserId)
				.OnDelete(DeleteBehavior.Cascade);

			// Cấu hình quan hệ 1-1 với Publisher
			builder.HasOne(u => u.Publisher)
				.WithOne(p => p.User)
				.HasForeignKey<Publisher>(p => p.UserId)
				.OnDelete(DeleteBehavior.Cascade);
		}
	}

}
