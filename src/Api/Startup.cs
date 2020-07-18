using DistanceService.Application.Queries;
using DistanceService.Infrastructure.IoC;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace DistanceService.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSwaggerGen(cfg =>
            {
                cfg.SwaggerDoc("v1", new Info
                {
                    Title = "Distance Service",
                    Version = "v1",
                    TermsOfService = "None"
                });
            });

            services
                .Configure<CTeleport.Client.Settings>(Configuration.GetSection("CTeleportSettings"));

            services
                .AddOptions()
                .AddHttpClient()
                .RegisterServices()
                .AddMediatR(typeof(GetDistanceBetweenAirportsQuery));

            services
                .AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment() || env.IsStaging())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger()
                    .UseSwaggerUI(cfg => cfg.SwaggerEndpoint("/swagger/v1/swagger.json", "v1"));
            }

            app.UseMvc();
        }
    }
}
