using Microsoft.Extensions.DependencyInjection;
using SubscriptionService.Application.Commands;
using SubscriptionService.Application.Commands.Interfaces;
using SubscriptionService.Application.Services;
using EventHandler = SubscriptionService.Application.Services.EventHandler;

namespace SubscriptionService.Application.Configuration
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IForumSubCommand, ForumSubCommand>();
            services.AddScoped<IPostSubCommand, PostSubCommand>();
            services.AddScoped<IEventHandler, EventHandler>();

            return services;
        }
    }
}
