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
}