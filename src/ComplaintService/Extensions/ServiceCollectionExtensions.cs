using System;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ComplaintService.BusinessDomain.ApplicationModels;
using ComplaintService.DataAccess.Contexts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace ComplaintService.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMicroserviceDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString(nameof(ComplaintDbContext));
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            services.AddDbContext<ComplaintDbContext>(builder => builder.UseSqlite(connectionString, sqlOptions => sqlOptions.MigrationsAssembly(migrationsAssembly)));
            return services;
        }

        public static IServiceCollection AddMicroserviceServicesAndOptions(this IServiceCollection services, IConfiguration configuration)
        {
            var microserviceSetting = configuration.GetSection(nameof(MicroserviceSetting)).Get<MicroserviceSetting>();
            services.AddSingleton(microserviceSetting);

            return services;
        }

        public static IServiceCollection AddMicroserviceAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var microserviceSetting = configuration.GetSection(nameof(MicroserviceSetting)).Get<MicroserviceSetting>();

            services.AddAuthentication(sharedOptions =>
            {
                sharedOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                sharedOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                sharedOptions.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(cfg =>
            {
                cfg.RequireHttpsMetadata = false;
                cfg.SaveToken = true;

                cfg.TokenValidationParameters = new TokenValidationParameters
                { 
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = microserviceSetting.Authority,
                    ValidAudience = microserviceSetting.ApiName,
                };

                cfg.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        Console.WriteLine("OnAuthenticationFailed: " + context.Exception.Message);
                        return Task.CompletedTask;
                    },
                    OnTokenValidated = context =>
                    {
                        Console.WriteLine("OnTokenValidated: " + context.SecurityToken);
                        return Task.CompletedTask;
                    }
                };
            });
            return services;
        }
    }
}

/*
 *
 * The data is NULL at ordinal 1. This method can't be called on NULL values. Check using IsDBNull before calling.

 */