using System.IdentityModel.Tokens.Jwt;
using AutoMapper;
using ComplaintService.DataAccess.Contexts;
using ComplaintService.Extensions;
using ComplaintService.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;

namespace ComplaintService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            IdentityModelEventSource.ShowPII = true;
            services.AddControllers(options => { options.Filters.Add(typeof(ValidateModelStateActionFilter)); });

            services.AddCors();
            services.AddAutoMapper(typeof(AutoMapping));
            services.AddMicroserviceDbContext(Configuration);
            services.AddMicroserviceServicesAndOptions(Configuration);
            services.AddMicroserviceAuthentication(Configuration);
            services.AddRepositoryPattern<ComplaintDbContext>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            app.UseConfigureSecurityHeaders(env);

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            app.UseWelcomePage();
        }
    }
}

//dotnet ef migrations add "initial migration" -c ComplaintDbContext -s ComplaintService -p ComplaintService.DataAccess  -o Migrations