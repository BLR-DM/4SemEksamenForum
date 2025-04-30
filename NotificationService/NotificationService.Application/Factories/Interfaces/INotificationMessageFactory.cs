using System.Text.Json;
using NotificationService.Application.EventDtos;

namespace NotificationService.Application.Factories.Interfaces
{
    public interface INotificationMessageFactory
    {
        Task<string> BuildMessageAsync(string topic, EventDto dto);
        string BuildForPostPublished(RequestedForumSubscribersCollectedEventDto dto);
    }
}