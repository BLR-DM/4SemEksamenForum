using Microsoft.Extensions.DependencyInjection;
using NotificationService.Application.Commands;
using NotificationService.Application.Commands.Interfaces;
using NotificationService.Application.Factories;
using NotificationService.Application.Factories.Interfaces;

namespace NotificationService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<INotificationCommand, NotificationCommand>();
        services.AddScoped<INotificationMessageFactory, NotificationMessageFactory>();

        return services;
    }
}