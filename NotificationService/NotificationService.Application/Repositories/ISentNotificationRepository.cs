using NotificationService.Domain.Entities;

namespace NotificationService.Application.Repositories
{
    public interface ISentNotificationRepository
    {
        Task AddAsync(SentNotification sentNotification);
        Task<SentNotification> GetSentNotificationAsync(string userId, int notificationId);
        Task PatchAsync();
    }
}