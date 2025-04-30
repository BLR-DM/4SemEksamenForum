using SubscriptionService.Application.EventDto;

namespace SubscriptionService.Application.Services
{
    public interface IPostPublishedHandler
    {
        Task HandlePostPublished(PostPublishedDto evtDto);
        Task HandlePostPublishedNotification(NotifyForumSubscriberEventDto evtDto);
    }
}