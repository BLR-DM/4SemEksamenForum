using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VoteService.Application.Queries.Interfaces;
using VoteService.Domain.Interfaces;
using VoteService.Infrastructure.Queries;
using VoteService.Infrastructure.Repositories;

namespace VoteService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddScoped<IPostVoteRepository, PostVoteRepository>();
            services.AddScoped<ICommentVoteRepository, CommentVoteRepository>();
            services.AddScoped<IPostVoteQuery, PostVoteQuery>();
            services.AddScoped<ICommentVoteQuery, CommentVoteQuery>();

            // Add-Migration InitialMigration -Context VoteContext -Project VoteService.DatabaseMigration
            // Update-Database -Context VoteContext -Project VoteService.DatabaseMigration
            services.AddDbContext<VoteContext>(options =>
                options.UseNpgsql(
                    configuration.GetConnectionString
                        ("VoteDbConnection"),
                    x =>
                        x.MigrationsAssembly("VoteService.DatabaseMigration")));

            return services;
        }
    }
}
