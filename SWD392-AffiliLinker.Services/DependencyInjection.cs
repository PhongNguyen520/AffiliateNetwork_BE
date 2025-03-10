using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SWD392_AffiliLinker.Repositories.IUOW;
using SWD392_AffiliLinker.Repositories.UOW;
using SWD392_AffiliLinker.Services.Interfaces;
using SWD392_AffiliLinker.Services.Mapping;
using SWD392_AffiliLinker.Services.Services;
using System.Text;

namespace SWD392_AffiliLinker.Services
{
    public static class DependencyInjection
	{
		public static void AddServiceConfig(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddServices();
			services.AddAutoMapperConfig();
		}


		public static void AddServices(this IServiceCollection services)
		{
			services.AddScoped<IAuthenticationService, AuthenticationService>();
			services.AddScoped<RoleManager<IdentityRole<Guid>>>();
			services.AddScoped<IJwtTokenService, JwtTokenService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            services.AddScoped<ICurrentUserService, CurrentUserService>();

        }

        public static void AddAutoMapperConfig(this IServiceCollection services)
		{
			services.AddAutoMapper(typeof(MappingProfile));
		}

    }
}
