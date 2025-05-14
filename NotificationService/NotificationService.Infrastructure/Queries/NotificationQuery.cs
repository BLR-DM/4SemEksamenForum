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
        try
        {
            var sentNotifications = await _context.SentNotifications
                .AsNoTracking()
                .Where(sn => sn.UserId == userId)
                .ToListAsync();

            var notificationIds = sentNotifications.Select(sn => sn.NotificationId).ToList();

            // Pull notifications from DB
            var notifications = await _context.Notifications
                .AsNoTracking()
                .Where(n => notificationIds.Contains(n.Id))
                .ToListAsync();

            // Join in memory
            var notificationDtos = notifications.Select(n => new NotificationDto
            {
                Id = n.Id,
                Message = n.Message,
                IsRead = sentNotifications.FirstOrDefault(sn => sn.NotificationId == n.Id)?.IsRead ?? false,
                CreatedAt = n.CreatedAt,
                SourceId = n.SourceId,
                SourceType = n.SourceType,
                ContextId = n.ContextId,
                ContextType = n.ContextType
            }).ToList();


            return notificationDtos;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}