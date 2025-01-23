using Microsoft.OpenApi.Models;

namespace SWD392_AffiliLinker.API
{
	public static class DependencyInjection
	{
		public static void AddConfig(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddSwaggerUIAuthentication();
		}

		public static void AddSwaggerUIAuthentication(this IServiceCollection services)
		{
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "SWD392-AffiliLinker.API", Version = "v1" });
				c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
				{
					Description = "Example: \"Authorization: Bearer {token}\"",
					Name = "Authorization",
					In = ParameterLocation.Header,
					Type = SecuritySchemeType.ApiKey,
					Scheme = "Bearer"
				});
				c.AddSecurityRequirement(new OpenApiSecurityRequirement
				{
					{
						new OpenApiSecurityScheme
						{
							Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
						},
						Array.Empty<string>()
					}
				});
			});
		}
	}
}
