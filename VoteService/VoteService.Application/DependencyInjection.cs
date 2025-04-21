using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using VoteService.Application.Commands;
using VoteService.Application.Interfaces;
using VoteService.Application.Services;
using VoteService.Domain.Services;
using EventHandler = VoteService.Application.Services.EventHandler;

namespace VoteService.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IPostVoteCommand, PostVoteCommand>();
            services.AddScoped<ICommentVoteCommand, CommentVoteCommand>();
            services.AddScoped<PostVoteService>();
            services.AddScoped<CommentVoteService>();
            services.AddScoped<IEventHandler, EventHandler>();

            return services;
        }
    }
}
