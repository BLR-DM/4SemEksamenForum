using Microsoft.Extensions.DependencyInjection;
using PointService.Application.Command;
using PointService.Application.Services;
using EventHandler = PointService.Application.Services.EventHandler;


namespace PointService.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IPointActionCommand, PointActionCommand>();
            services.AddScoped<IPointEntryCommand, PointEntryCommand>();
            services.AddScoped<IEventHandler, EventHandler>();

            return services;
        }
    }
}