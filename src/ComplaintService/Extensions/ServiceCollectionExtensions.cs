using System;
using System.Reflection;
using System.Threading.Tasks;
using ComplaintService.BusinessDomain.ApplicationModels;
using ComplaintService.BusinessDomain.Services;
using ComplaintService.DataAccess.Contexts;
using ComplaintService.DataAccess.Repositories;
using ComplaintService.DataAccess.RepositoryPattern;
using ComplaintService.DataAccess.RepositoryPattern.Interfaces;
using CoreLibrary.DataContext;
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
            var migrationsAssembly = typeof(ComplaintDbContext).GetTypeInfo().Assembly.GetName().Name;

            services.AddDbContext<ComplaintDbContext>(builder => builder.UseSqlServer(connectionString, sqlOptions => sqlOptions.MigrationsAssembly(migrationsAssembly)));
            return services;
        }

        public static IServiceCollection AddMicroserviceServicesAndOptions(this IServiceCollection services, IConfiguration configuration)
        {
            var microserviceSetting = configuration.GetSection(nameof(MicroserviceSetting)).Get<MicroserviceSetting>();
            services.AddSingleton(microserviceSetting);

            services.AddTransient<IComplaintRepository, ComplaintRepository>();
            services.AddTransient<IComplaintService, BusinessDomain.Services.ComplaintService>();
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
                cfg.Audience = microserviceSetting.ApiName;
                cfg.Authority = microserviceSetting.Authority;

                // cfg.TokenValidationParameters = new TokenValidationParameters
                // {
                //     ValidateIssuer = true,
                //     ValidateAudience = true,
                //     ValidIssuer = microserviceSetting.Authority,
                //     ValidAudience = microserviceSetting.ApiName
                // };

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
        
        public static IServiceCollection AddRepositoryPattern<TApplicationContext>(this IServiceCollection services)
            where TApplicationContext : EntityFrameworkDataContext<TApplicationContext>
        {
            services.AddTransient<IDataContextAsync, TApplicationContext>();
            services.AddTransient<IUnitOfWorkAsync, EntityFrameorkUnitOfWork>();
            services.AddTransient<IUnitOfWork, EntityFrameorkUnitOfWork>();
            services.AddTransient(typeof(IRepositoryAsync<>), typeof(Repository<>));
            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
            return services;
        }
    }
}