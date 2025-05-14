using Microsoft.EntityFrameworkCore;
using NotificationService.Application.Repositories;
using NotificationService.Domain.Entities;

namespace NotificationService.Infrastructure.Repositories
{
    public class SentNotificationRepository : ISentNotificationRepository
    {
        private readonly NotificationContext _context;

        public SentNotificationRepository(NotificationContext context)
        {
            _context = context;
        }
        async Task ISentNotificationRepository.AddAsync(SentNotification sentNotification)
        {
            _context.SentNotifications.Add(sentNotification);
            await _context.SaveChangesAsync();
        }

        async Task<SentNotification> ISentNotificationRepository.GetSentNotificationAsync(string userId, int notificationId)
        {
            return await _context.SentNotifications.FirstAsync(sn => sn.UserId == userId && sn.NotificationId == notificationId);
        }

        async Task ISentNotificationRepository.PatchAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}