using NotificationService.Application.Commands.CommandDto;

namespace NotificationService.Application.Commands.Interfaces
{
    public interface ISentNotificationCommand
    {
        Task CreateSentNotificationAsync(CreateSentNotificationDto dto);
        Task MarkAsReadAsync(MarkAsReadDto dto);
    }
}