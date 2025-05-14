using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using NotificationService.Application.Queries;

namespace NotificationService.Infrastructure.Queries;

public class NotificationQuery : INotificationQuery
{
    private readonly NotificationContext _context;

    public NotificationQuery(NotificationContext context)
    {
        _context = context;
    }
    async Task<List<NotificationDto>> INotificationQuery.GetNotificationsForUserAsync(string userId)
    {
        var notifications = await _context.Notifications
            .AsNoTracking()
            .Where(n => n.UserId == userId)
            .ToListAsync();

        var notificationDtos = notifications.Select(n => 
            new NotificationDto(n.Id, n.UserId, n.Message, n.NotificationRead, n.CreatedAt)).ToList();

        return notificationDtos;
    }
}