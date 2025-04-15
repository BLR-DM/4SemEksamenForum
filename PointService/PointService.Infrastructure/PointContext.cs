using Microsoft.EntityFrameworkCore;
using PointService.Domain.Entities;

namespace PointService.Infrastructure
{
    public class PointContext : DbContext
    {
        public PointContext(DbContextOptions<PointContext> options) : base(options) { }

        public DbSet<PointAction> PointActions { get; set; }
        public DbSet<PointEntry> PointEntries { get; set; }
    }
}