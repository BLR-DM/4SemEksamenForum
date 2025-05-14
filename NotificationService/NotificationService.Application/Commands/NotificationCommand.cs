using NotificationService.Application.Commands.Interfaces;
using NotificationService.Application.Repositories;
using NotificationService.Application.Services;
using NotificationService.Domain.Entities;

namespace NotificationService.Application.Commands;

public class NotificationCommand : INotificationCommand
{
    private readonly INotificationRepository _repository;
    private readonly IEventHandler _eventHandler;

    public NotificationCommand(INotificationRepository repository, IEventHandler eventHandler)
    {
        _repository = repository;
        _eventHandler = eventHandler;
    }
    async Task<int> INotificationCommand.CreateNotificationAsync(string message, int sourceId, string sourceType, int contextId, string contextType)
    {
        try
        {
            var notification = Notification.Create(message, sourceId, sourceType, contextId, contextType);

            await _repository.AddAsync(notification);

            return notification.Id;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    //async Task INotificationCommand.MarkAsReadAsync(int id, string userId)
    //{
    //    var notification = await _repository.GetNotificationAsync(id);

    //    if (notification.IsRead)
    //        return;

    //    notification.MarkAsRead();

    //    await _repository.PatchAsync();
    //}
}