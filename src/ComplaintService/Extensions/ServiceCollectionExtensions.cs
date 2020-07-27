using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using ComplaintService.BusinessDomain.ApplicationModels;
using ComplaintService.BusinessDomain.Services;
using ComplaintService.DataAccess.Contexts;
using ComplaintService.DataAccess.Repositories;
using ComplaintService.DataAccess.RepositoryPattern;
using ComplaintService.DataAccess.RepositoryPattern.Interfaces;
using ComplaintService.Filters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

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

        public static IServiceCollection AddRepositoryPattern(this IServiceCollection services)
        {
            services.AddTransient<IDataContextAsync, ComplaintDbContext>();
            services.AddTransient<IUnitOfWorkAsync, EntityFrameorkUnitOfWork>();
            services.AddTransient<IUnitOfWork, EntityFrameorkUnitOfWork>();
            services.AddTransient(typeof(IRepositoryAsync<>), typeof(Repository<>));
            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
            return services;
        }

        public static IServiceCollection AddSwaggerDoc(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "Complaint Service", Version = "v1"});
                c.SchemaFilter<EnumSchemaFilter>();
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
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
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
                });
            });
            services.AddSwaggerGenNewtonsoftSupport();
            return services;
        }
    }
}