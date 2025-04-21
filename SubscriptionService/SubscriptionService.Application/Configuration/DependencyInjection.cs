using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SubscriptionService.Application.Commands;
using SubscriptionService.Application.Commands.Interfaces;
using SubscriptionService.Application.Services;

namespace SubscriptionService.Application.Configuration
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IForumSubCommand, ForumSubCommand>();
            services.AddScoped<IPostSubCommand, PostSubCommand>();
            services.AddScoped<IEventHandler, SubscriptionService.Application.Services.EventHandler>();

            return services;
        }
    }
}
