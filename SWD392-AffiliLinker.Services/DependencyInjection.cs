using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SWD392_AffiliLinker.Repositories.IUOW;
using SWD392_AffiliLinker.Repositories.UOW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD392_AffiliLinker.Services
{
    public static class DependencyInjection
	{
		public static void AddInAddInfrastructure(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddRepositories();
			services.AddServices();
		}

		public static void AddRepositories(this IServiceCollection services)
		{
			services.AddScoped<IUnitOfWork, UnitOfWork>();
		}

		public static void AddServices(this IServiceCollection services)
		{
		}
	}
}
