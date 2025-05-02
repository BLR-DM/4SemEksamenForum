using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NotificationService.Application.Queries;
using NotificationService.Application.Repositories;
using NotificationService.Application.Services;
using NotificationService.Infrastructure.Queries;
using NotificationService.Infrastructure.Repositories;
using NotificationService.Infrastructure.Services;

namespace NotificationService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<INotificationQuery, NotificationQuery>();
        services.AddScoped<INotificationRepository, NotificationRepository>();
        services.AddScoped<IPublisherService, DaprPublisherService>();

        // Add-Migration InitialMigration -Context NotificationContext -Project NotificationService.DatabaseMigration
        // Update-Database -Context NotificationContext -Project NotificationService.DatabaseMigration
        services.AddDbContext<NotificationContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString
                    ("NotificationDbConnection"),
                x =>
                    x.MigrationsAssembly("NotificationService.DatabaseMigration")));

        return services;
    }
}