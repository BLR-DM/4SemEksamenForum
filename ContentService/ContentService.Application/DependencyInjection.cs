using ContentService.Application.Commands;
using ContentService.Application.Commands.Interfaces;
using ContentService.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using EventHandler = ContentService.Application.Services.EventHandler;

namespace ContentService.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IForumCommand, ForumCommand>();
            services.AddScoped<IPostCommand, PostCommand>();
            services.AddScoped<IEventHandler, EventHandler>();

            return services;
        }
    }
}