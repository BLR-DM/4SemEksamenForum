using ContentService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ContentService.Infrastructure
{
    public class ContentContext : DbContext
    {
        public ContentContext(DbContextOptions<ContentContext> options) : base(options)
        {
        }
        
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Forum> Forums { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Forum

            modelBuilder.Entity<Forum>()
                .Property(f => f.Status)
                .HasConversion<string>(); // Stores enum as a string

            modelBuilder.Entity<Forum>()
                .Property(f => f.CreatedDate)
                .HasColumnType("timestamp(0) without time zone");

            // Post

            modelBuilder.Entity<Post>()
                .Property(f => f.CreatedDate)
                .HasColumnType("timestamp(0) without time zone");

            // Comment

            modelBuilder.Entity<Comment>()
                .Property(f => f.CreatedDate)
                .HasColumnType("timestamp(0) without time zone");


            // Table mappings on entities
            modelBuilder.Entity<Post>().ToTable("Posts");
            modelBuilder.Entity<Comment>().ToTable("Comments");
            modelBuilder.Entity<Forum>().ToTable("Forums");
        }
    }
}
