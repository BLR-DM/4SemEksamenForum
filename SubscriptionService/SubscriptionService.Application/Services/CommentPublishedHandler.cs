using SubscriptionService.Application.Commands.Interfaces;
using SubscriptionService.Application.EventDto;
using SubscriptionService.Application.Queries.Interfaces;

namespace SubscriptionService.Application.Services
{
    public class CommentPublishedHandler : ICommentPublishedHandler
    {
        private readonly IPostSubCommand _postCommand;
        private readonly IEventHandler _eventHandler;
        private readonly IPostSubQuery _query;

        public CommentPublishedHandler(IPostSubCommand postCommand, IEventHandler eventHandler, IPostSubQuery query)
        {
            _postCommand = postCommand;
            _eventHandler = eventHandler;
            _query = query;
        }
        async Task ICommentPublishedHandler.HandleCommentPublished(CommentPublishedDto evtDto)
        {
            await _postCommand.CreateAsync(evtDto.PostId, evtDto.UserId);

            await _eventHandler.PublishPostNotificationRequest(evtDto.ForumId, evtDto.PostId);
        }

        async Task ICommentPublishedHandler.HandleCommentPublishedNotification(NotifyPostSubscriberEventDto evtDto)
        {
            var subscribers = await _query.GetSubscriptionsByPostIdAsync(evtDto.PostId);

            if (!subscribers.Any())
                return;

            var tasks = subscribers.Select(sub =>
                _eventHandler.NotifyPostSubscriber(sub, evtDto.ForumId, evtDto.PostId));

            await Task.WhenAll(tasks);
        }
    }
}