using SubscriptionService.Application.Commands.Interfaces;
using SubscriptionService.Application.EventDto;
using SubscriptionService.Application.Queries.Interfaces;

namespace SubscriptionService.Application.Services
{
    public class PostPublishedHandler : IPostPublishedHandler
    {
        private readonly IForumSubCommand _forumCommand;
        private readonly IPostSubCommand _postCommand;
        private readonly IEventHandler _eventHandler;
        private readonly IForumSubQuery _query;

        public PostPublishedHandler(IForumSubCommand forumCommand, IPostSubCommand postCommand, IEventHandler eventHandler, IForumSubQuery query)
        {
            _forumCommand = forumCommand;
            _postCommand = postCommand;
            _eventHandler = eventHandler;
            _query = query;
        }
        async Task IPostPublishedHandler.HandlePostPublished(PostPublishedDto evtDto)
        {
            await _forumCommand.CreateAsync(evtDto.ForumId, evtDto.UserId);
            await _postCommand.CreateAsync(evtDto.PostId, evtDto.UserId);

            await _eventHandler.PublishForumNotificationRequest(evtDto.ForumId, evtDto.PostId);
        }

        async Task IPostPublishedHandler.HandlePostPublishedNotification(NotifyForumSubscriberEventDto evtDto)
        {
            var subscribers = await _query.GetSubscriptionsByForumIdAsync(evtDto.ForumId);

            if (!subscribers.Any())
                return;

            //foreach (var sub in subscribers)
            //    await _eventHandler.PublishNotifySubscriber(sub, evtDto.ForumId, evtDto.PostId);

            var tasks = subscribers.Select(sub =>
                _eventHandler.PublishNotifyForumSubscriber(sub, evtDto.ForumId, evtDto.PostId));

            await Task.WhenAll(tasks);
        }
    }
}