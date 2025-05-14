using Microsoft.EntityFrameworkCore;
using NotificationService.Application.Repositories;
using NotificationService.Domain.Entities;

namespace NotificationService.Infrastructure.Repositories;

public class NotificationRepository : INotificationRepository
{
    private readonly NotificationContext _context;

    public NotificationRepository(NotificationContext context)
    {
        _context = context;
    }
    async Task INotificationRepository.AddAsync(Notification notification)
    {
        _context.Notifications.Add(notification);
        await _context.SaveChangesAsync();
    }

    async Task<Notification> INotificationRepository.GetNotificationAsync(int id)
    {
        return await _context.Notifications.FirstAsync(n => n.Id == id);
    }

    async Task INotificationRepository.PatchAsync()
    {
        await _context.SaveChangesAsync();
    }
}