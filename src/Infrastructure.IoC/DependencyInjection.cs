using CTeleport.Client;
using CTeleport.Client.Cache;
using CTeleport.Client.Interfaces;
using DistanceService.Domain.Services;
using DistanceService.Infrastructure.Data.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DistanceService.Infrastructure.IoC
{
    public static class DependencyInjection
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddSingleton<ICache, Cache>();
            services.AddTransient<ICTeleportClient, CTeleportClient>();
            services.AddTransient<IAirportService, AirportService>();
            services.AddTransient<IDistanceService, Application.Services.DistanceService>();

            return services;
        }
    }
}
