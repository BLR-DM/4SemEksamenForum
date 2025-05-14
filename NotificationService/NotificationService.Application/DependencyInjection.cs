using Microsoft.Extensions.DependencyInjection;
using NotificationService.Application.Commands;
using NotificationService.Application.Commands.Interfaces;
using NotificationService.Application.Helpers;
using NotificationService.Application.Services;
using EventHandler = NotificationService.Application.Services.EventHandler;

namespace NotificationService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<INotificationCommand, NotificationCommand>();
        services.AddScoped<IEventHandler, EventHandler>();
        services.AddScoped<ISentNotificationCommand, SentNotificationCommand>();
        services.AddScoped<INotificationHandler, NotificationHandler>();

        return services;
    }
}