using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using VoteService.Application.Commands;
using VoteService.Application.Interfaces;
using VoteService.Domain.Services;

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

            return services;
        }
    }
}
