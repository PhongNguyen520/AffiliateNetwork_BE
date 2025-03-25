using CloudinaryDotNet;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SWD392_AffiliLinker.Services.Config.LinkConfig;
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
			services.AddOtherServiceConfig(configuration);


            var cloudinarySettings = configuration.GetRequiredSection("Cloudinary");
            var account = new Account(
                cloudinarySettings["CloudName"],
                cloudinarySettings["ApiKey"],
                cloudinarySettings["ApiSecret"]
            );
            var cloudinary = new Cloudinary(account);

            // Đăng ký Cloudinary vào DI container
            services.AddSingleton(cloudinary);
        }



		public static void AddServices(this IServiceCollection services)
		{
			services.AddScoped<IAuthenticationService, AuthenticationService>();
			services.AddScoped<RoleManager<IdentityRole<Guid>>>();
			services.AddScoped<IJwtTokenService, JwtTokenService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
			services.AddScoped<IAffiliateLinkService, AffiliateLinkService>();
			services.AddScoped<ICurrentUserService, CurrentUserService>();
			services.AddScoped<ICampaignService, CampaignService>();
			services.AddScoped<ICampaignMemberService, CampaignMemberService>();
			services.AddScoped<IPayoutModelService, PayoutModelService>();
			services.AddScoped<ICategoryService, CategoryService>();
			services.AddScoped<IVnPayService, VnPayService>();
			services.AddScoped<IClickService, ClickService>();
			services.AddScoped<ICookieService, CookieService>();
			services.AddScoped<IExportTrafficService, ExportTrafficService>();
			services.AddScoped<IConversionService, ConversionService>();
			services.AddScoped<IHepperUploadImage, HepperUploadImage>();
			services.AddScoped<IAccountService, AccountService>();
			services.AddScoped<ICommissionService, CommissionService>();

		}

        public static void AddAutoMapperConfig(this IServiceCollection services)
		{
			services.AddAutoMapper(typeof(MappingProfile));
		}

		public static void AddOtherServiceConfig(this IServiceCollection services, IConfiguration configuration)
		{
			services.Configure<AffiliateDomainOptions>(configuration.GetSection("AffiliateDomain"));
		}
	}
}
