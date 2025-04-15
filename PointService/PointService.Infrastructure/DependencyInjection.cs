using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace PointService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<PointContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("PointDbConnection"), 
                    x => x.MigrationsAssembly("PointService.DatabaseMigration")));

            // Add-Migration InitialMigration -Context PointContext -Project PointService.DatabaseMigration
            // Update-Database -Context PointContext -Project PointService.DatabaseMigration
            return services;
        }
    }
}