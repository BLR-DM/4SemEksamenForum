using Microsoft.Extensions.DependencyInjection;
using PointService.Application.Command;


namespace PointService.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IPointActionCommand, PointActionCommand>();
            services.AddScoped<IPointEntryCommand, PointEntryCommand>();

            return services;
        }
    }
}