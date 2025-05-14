namespace NotificationService.Application.Commands.Interfaces;

public interface INotificationCommand
{
    Task<int> CreateNotificationAsync(string message, int sourceId, string sourceType, int contextId, string contextType);
    //Task MarkAsReadAsync(int id, string userId);
}