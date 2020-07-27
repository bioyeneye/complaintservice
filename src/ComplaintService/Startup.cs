using System.IdentityModel.Tokens.Jwt;
using AutoMapper;
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
            services.AddHttpContextAccessor();
            services.AddAutoMapper(typeof(AutoMapping));
            services.AddMicroserviceDbContext(Configuration);
            services.AddRepositoryPattern();
            services.AddMicroserviceServicesAndOptions(Configuration);
            services.AddMicroserviceAuthentication(Configuration);
            services.AddSwaggerDoc();
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

            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"); });


            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            app.UseWelcomePage();
        }
    }
}

//dotnet ef migrations add "initial migration" -c ComplaintDbContext -s ComplaintService -p ComplaintService.DataAccess  -o Migrations