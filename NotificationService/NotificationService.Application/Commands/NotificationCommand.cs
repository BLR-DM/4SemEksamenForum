using NotificationService.Application.Commands.Interfaces;
using NotificationService.Application.Repositories;
using NotificationService.Domain.Entities;

namespace NotificationService.Application.Commands;

public class NotificationCommand : INotificationCommand
{
    private readonly INotificationRepository _repository;

    public NotificationCommand(INotificationRepository repository)
    {
        _repository = repository;
    }
    async Task INotificationCommand.CreateNotificationAsync(string userId, string message)
    {
        var notification = Notification.Create(userId, message);

        await _repository.AddAsync(notification);
    }

    async Task INotificationCommand.MarkAsReadAsync(int id, string userId)
    {
        var notification = await _repository.GetNotificationAsync(id);

        if (notification.IsRead)
            return;

        notification.MarkAsRead();

        await _repository.PatchAsync();
    }
}