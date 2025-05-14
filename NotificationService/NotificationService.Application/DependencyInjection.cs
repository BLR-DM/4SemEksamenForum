using Microsoft.Extensions.DependencyInjection;
using NotificationService.Application.Commands;
using NotificationService.Application.Commands.Interfaces;
using NotificationService.Application.Factories.Interfaces;
using NotificationService.Application.Helpers;
using NotificationService.Application.Services;
using EventHandler = NotificationService.Application.Services.EventHandler;

namespace NotificationService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<INotificationCommand, NotificationCommand>();
        services.AddScoped<INotificationMessageFactory, MessageBuilder>();
        services.AddScoped<IEventHandler, EventHandler>();

        return services;
    }
}