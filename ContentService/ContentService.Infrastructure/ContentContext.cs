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
                .HasIndex(f => f.ForumName)
                .IsUnique();

            // Post
            modelBuilder.Entity<Post>()
                .Property(p => p.Status)
                .HasConversion<string>(); // Stores enum as a string

            // Comment
            modelBuilder.Entity<Comment>()
                .Property(c => c.Status)
                .HasConversion<string>(); // Stores enum as a string


            // Table mappings on entities
            modelBuilder.Entity<Post>().ToTable("Posts");
            modelBuilder.Entity<Comment>().ToTable("Comments");
            modelBuilder.Entity<Forum>().ToTable("Forums");
        }
    }
}
