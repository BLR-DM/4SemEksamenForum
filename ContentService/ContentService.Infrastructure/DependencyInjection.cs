using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ContentService.Application;
using ContentService.Application.Queries;
using ContentService.Application.Services;
using ContentService.Infrastructure.Helpers;
using ContentService.Infrastructure.Interfaces;
using ContentService.Infrastructure.Queries;
using ContentService.Infrastructure.Repositories;
using ContentService.Infrastructure.Services;

namespace ContentService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddScoped<IForumRepository, ForumRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork<ContentContext>>();
            services.AddScoped<IForumMapper, ForumMapper>();
            services.AddScoped<IForumQuery, ForumQuery>();
            services.AddScoped<IPublisherService, DaprPublisherService>();
            services.AddScoped<IModerationResultHandler, ModerationResultHandler>();

            // Add-Migration InitialMigration -Context ContentContext -Project ContentService.DatabaseMigration
            // Update-Database -Context ContentContext -Project ContentService.DatabaseMigration
            services.AddDbContext<ContentContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("ContentDbConnection"),
                    x => x.MigrationsAssembly("ContentService.DatabaseMigration")));

            return services;
        }
    }
}
