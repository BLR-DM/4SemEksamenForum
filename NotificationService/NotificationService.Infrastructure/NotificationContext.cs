using Microsoft.EntityFrameworkCore;
using NotificationService.Domain.Entities;

namespace NotificationService.Infrastructure;

public class NotificationContext : DbContext
{
    public NotificationContext(DbContextOptions<NotificationContext> options) : base(options) { }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<SentNotification> SentNotifications { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SentNotification>().HasKey(sn => new { sn.NotificationId, sn.UserId });
    }

}