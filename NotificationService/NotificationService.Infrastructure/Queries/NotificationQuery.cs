using Microsoft.EntityFrameworkCore;
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
        var notificationDtos = await _context.Notifications
            .AsNoTracking()
            .Where(n => n.UserId == userId)
            .Select(n => new NotificationDto
            (
                n.Id, 
                n.UserId, 
                n.Message, 
                n.IsRead, 
                n.CreatedAt
            )).ToListAsync();

        return notificationDtos;
    }
}