using Microsoft.EntityFrameworkCore;
using SWD392_AffiliLinker.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SWD392_AffiliLinker.Repositories.Context
{
	public class DatabaseContext : DbContext
	{
		public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }
		public virtual DbSet<Campaign> Classes => Set<Campaign>();
	}
}
