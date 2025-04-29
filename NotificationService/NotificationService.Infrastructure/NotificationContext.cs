using Microsoft.EntityFrameworkCore;
using NotificationService.Domain.Entities;

namespace NotificationService.Infrastructure;

public class NotificationContext : DbContext
{
    public NotificationContext(DbContextOptions<NotificationContext> options) : base(options) { }
    public DbSet<Notification> Notifications { get; set; }
    
}