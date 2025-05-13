using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PointService.Application.Queries;
using PointService.Application.Repositories;
using PointService.Infrastructure.Queries;
using PointService.Infrastructure.Repositories;

namespace PointService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddScoped<IPointActionRepository, PointActionRepository>();
            services.AddScoped<IPointEntryRepository, PointEntryRepository>();
            services.AddScoped<IPointEntryQuery, PointEntryQuery>();
            services.AddScoped<IPointActionQuery, PointActionQuery>();

            services.AddDbContext<PointContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("PointDbConnection"), 
                    x => x.MigrationsAssembly("PointService.DatabaseMigration")));

            // Add-Migration InitialMigration -Context PointContext -Project PointService.DatabaseMigration
            // Update-Database -Context PointContext -Project PointService.DatabaseMigration
            return services;
        }
    }
}