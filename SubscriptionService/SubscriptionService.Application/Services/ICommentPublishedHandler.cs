using SubscriptionService.Application.EventDto;

namespace SubscriptionService.Application.Services
{
    public interface ICommentPublishedHandler
    {
        Task HandleCommentPublished(CommentPublishedDto evtDto);
        Task HandleCommentPublishedNotification(NotifyPostSubscriberEventDto evtDto);
    }
}