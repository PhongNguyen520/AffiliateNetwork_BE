using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SWD392_AffiliLinker.Repositories.Context;
using SWD392_AffiliLinker.Repositories.IUOW;
using SWD392_AffiliLinker.Repositories.Repositories;
using SWD392_AffiliLinker.Repositories.UOW;

namespace SWD392_AffiliLinker.Repositories
{
	public static class DependencyInjection
	{
		public static void AddRepositoryConfig(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddRepositories();
		}

		public static void AddRepositories(this IServiceCollection services)
		{
			services.AddScoped<IUnitOfWork, UnitOfWork>();
			services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
		}
	}
}
