using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SWD392_AffiliLinker.Repositories.Context;
using SWD392_AffiliLinker.Repositories.Entities;
using SWD392_AffiliLinker.Services.Interfaces;
using SWD392_AffiliLinker.Services.Mapping;
using SWD392_AffiliLinker.Services.Services;
using System.Reflection;
using System.Text;

namespace SWD392_AffiliLinker.API
{
	public static class DependencyInjection
	{
		public static void AddConfig(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddSwaggerUIAuthentication();
			services.ConfigureIdentity();
			services.AddAuthenticationBearer(configuration);
			services.AddDatabase(configuration);
            services.AddCors();
        }

		public static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddDbContext<DatabaseContext>(options =>
			{
				options.UseSqlServer(configuration.GetConnectionString("MyDB"));
			});
		}

		public static void ConfigureIdentity(this IServiceCollection services)
		{
			services.AddIdentity<User, IdentityRole<Guid>>(options =>
			{
				options.Password.RequireDigit = true;
				options.Password.RequiredLength = 8;
				options.Password.RequireNonAlphanumeric = true;
				options.Password.RequireUppercase = true;
				options.Password.RequireLowercase = true;
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;

                //Config SigIn
                options.SignIn.RequireConfirmedPhoneNumber = false;
                options.SignIn.RequireConfirmedEmail = false;

                #region cmt code Tam
                //options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                //options.Lockout.MaxFailedAccessAttempts = 5;
                //options.Lockout.AllowedForNewUsers = true;
                //options.User.RequireUniqueEmail = true;
                //options.SignIn.RequireConfirmedEmail = false;
                //options.SignIn.RequireConfirmedPhoneNumber = false;
                #endregion
            })
			.AddEntityFrameworkStores<DatabaseContext>()
			.AddDefaultTokenProviders();
		}

		public static void AddAuthenticationBearer(this IServiceCollection services, IConfiguration configuration)
		{
            
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,

                    ValidAudience = configuration["JWT:ValidAudience"],
                    ValidIssuer = configuration["JWT:ValidIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]))

                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var path = context.HttpContext.Request.Path;
                        if (path.StartsWithSegments("/chatHub") &&
                            context.Request.Query.TryGetValue("access_token", out var accessToken))
                        {
                            context.Token = accessToken.ToString().Replace("Bearer ", "", StringComparison.OrdinalIgnoreCase).Trim();
                        }
                        return Task.CompletedTask;
                    },
                    OnAuthenticationFailed = context =>
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        return Task.CompletedTask;
                    }
                };
            });
        }

        public static void AddSwaggerUIAuthentication(this IServiceCollection services)
		{
			services.AddSwaggerGen(c =>
			{
				var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
				c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

				c.SwaggerDoc("v1", new OpenApiInfo { Title = "SWD392-AffiliLinker.API", Version = "v1" });

				c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
				{
					Description = "Enter JWT token: Bearer {token}",
					Name = "Authorization",
					In = ParameterLocation.Header,
					Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT",
					Scheme = "Bearer"
				});

				c.AddSecurityRequirement(new OpenApiSecurityRequirement
			{
				{
					new OpenApiSecurityScheme
					{
						Reference = new OpenApiReference 
                        { 
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
					},
                    new string[]{}
                }
			});
			});
		}

        public static void AddCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigins", policy =>
                {
                    policy.WithOrigins("http://localhost:3000", "https://affiliate-networking.vercel.app")
                          .AllowAnyMethod()
                          .AllowAnyHeader()
                          .AllowCredentials();
                });
            });
        }
    }

}


