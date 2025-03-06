using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SWD392_AffiliLinker.Repositories.Context.EntityConfig;
using SWD392_AffiliLinker.Repositories.Entities;

namespace SWD392_AffiliLinker.Repositories.Context
{
	public class DatabaseContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
	{
		public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }
		public virtual DbSet<User> Users { get; set; }
		public virtual DbSet<Advertiser> Advertisers { get; set; }
		public virtual DbSet<AffiliateLink> AffiliateLinks { get; set; }
		public virtual DbSet<Bank> Banks { get; set; }
		public virtual DbSet<Campaign> Campaigns { get; set; }
		public virtual DbSet<CampaignCategory> CampaignCategories { get; set; }
		public virtual DbSet<CampaignMember> CampaignMembers { get; set; }
		public virtual DbSet<ClickCount> ClickCount { get; set; }
		public virtual DbSet<ClickInfo> ClickInfo { get; set; }
		public virtual DbSet<Commission> Commissions { get; set; }
		public virtual DbSet<Conversion> Conversion { get; set; }
		public virtual DbSet<Feedback> Feedbacks { get; set; }
		public virtual DbSet<PayoutModel> PayoutModels { get; set; }
		public virtual DbSet<Publisher> Publisher { get; set; }
		public virtual DbSet<Report> Reports { get; set; }
		public virtual DbSet<ReportMedia> ReportMedia { get; set; }
		public virtual DbSet<Transaction> Transaction { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);


            //Remove the AspNet prefix
            #region Remove name
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var tableName = entityType.GetTableName();
                if (tableName.StartsWith("AspNet"))
                {
                    entityType.SetTableName(tableName.Substring(6));
                }
            }
            #endregion



            modelBuilder.ApplyConfiguration(new AdvertiserConfiguration());
			modelBuilder.ApplyConfiguration(new AdvertiserConfiguration());
			modelBuilder.ApplyConfiguration(new BankConfiguration());
			modelBuilder.ApplyConfiguration(new CampaignConfiguration());
			modelBuilder.ApplyConfiguration(new CampaignCategoryConfiguration());
			modelBuilder.ApplyConfiguration(new CampaignMemberConfiguration());
			modelBuilder.ApplyConfiguration(new CampaignPayoutModelConfiguration());
			modelBuilder.ApplyConfiguration(new ClickCountConfiguration());
			modelBuilder.ApplyConfiguration(new ClickInfoConfiguration());
			modelBuilder.ApplyConfiguration(new CommissionConfiguration());
			modelBuilder.ApplyConfiguration(new ConversionConfiguration());
			modelBuilder.ApplyConfiguration(new FeedbackConfiguration());
			modelBuilder.ApplyConfiguration(new NotificationConfiguration());
			modelBuilder.ApplyConfiguration(new PayoutModelConfiguration());
			modelBuilder.ApplyConfiguration(new PublisherConfiguration());
			modelBuilder.ApplyConfiguration(new ReportConfiguration());
			modelBuilder.ApplyConfiguration(new ReportMediaConfiguration());
			modelBuilder.ApplyConfiguration(new TransactionConfiguration());
			modelBuilder.ApplyConfiguration(new UserConfiguration());
		}
	}
}
