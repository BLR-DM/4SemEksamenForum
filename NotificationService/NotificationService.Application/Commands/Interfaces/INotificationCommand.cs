namespace NotificationService.Application.Commands.Interfaces;

public interface INotificationCommand
{
    Task CreateNotificationAsync(string userId, string message);
}