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
        return await _context.Notifications
            .AsNoTracking()
            .Where(n => n.UserId == userId)
            .Select(n => new NotificationDto(n.Message, n.CreatedAt))
            .ToListAsync();
    }
}