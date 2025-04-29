using NotificationService.Domain.Entities;

namespace NotificationService.Application.Repositories;

public interface INotificationRepository
{
    Task AddAsync(Notification notification);
}