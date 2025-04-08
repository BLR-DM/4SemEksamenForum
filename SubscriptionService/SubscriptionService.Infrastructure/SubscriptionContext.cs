using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SubscriptionService.Domain.Entities;

namespace SubscriptionService.Infrastructure
{
    public class SubscriptionContext : DbContext
    {
        public SubscriptionContext(DbContextOptions<SubscriptionContext> options) : base(options)
        {
        }

        public DbSet<PostSubscription> PostSubscriptions { get; set; }
        public DbSet<ForumSubscription> ForumSubscriptions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PostSubscription>().HasKey(ps => new { ps.PostId, ps.AppUserId });

            modelBuilder.Entity<ForumSubscription>().HasKey(fs => new { fs.ForumId, fs.AppUserId });
        }
    }
}
