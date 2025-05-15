
using SubscriptionService.Application.EventDto;

namespace SubscriptionService.Application.Services
{
    public class EventHandler : IEventHandler
    {
        private readonly IPublisherService _publisherService;

        public EventHandler(IPublisherService publisherService)
        {
            _publisherService = publisherService;
        }

        // Forum
        async Task IEventHandler.UserSubscribedToForum(string userId, int subscriptionId, int forumId)
        {
            var evtDto = new UserSubscribedToForumEventDto(userId, subscriptionId, forumId);
            await _publisherService.PublishEvent("user-subscribed-to-forum", evtDto);
        }

        async Task IEventHandler.FailedToSubscribeUserOnForumPublished(string userId, int forumId)
        {
            var evtDto = new FailedToSubscribeUserToForumEventDto(userId, forumId);
            await _publisherService.PublishEvent("failed-to-subscribe-user-on-forum-published", evtDto);
        }

        async Task IEventHandler.FailedToSubscribeUserOnPostPublished(string userId, int forumId, int postId)
        {
            var evtDto = new FailedToSubscribeUserToPostEventDto(userId, forumId, postId);
            await _publisherService.PublishEvent("failed-to-subscribe-user-on-post-published", evtDto);
        }

        async Task IEventHandler.UserUnsubscribedFromForum(string userId, int subscriptionId, int forumId)
        {
            var evtDto = new UserUnSubscribedFromForumEventDto(userId, subscriptionId, forumId);
            await _publisherService.PublishEvent("user-unsubscribed-from-forum", evtDto);
        }

        // Post
        async Task IEventHandler.UserSubscribedToPost(string userId, int subscriptionId, int postId)
        {
            var evtDto = new UserSubscribedToPostEventDto(userId, subscriptionId, postId);
            await _publisherService.PublishEvent("user-subscribed-to-post", evtDto);
        }

        async Task IEventHandler.UserUnsubscribedFromPost(string userId, int subscriptionId, int postId)
        {
            var evtDto = new UserUnSubscribedFromPostEventDto(userId, subscriptionId, postId);
            await _publisherService.PublishEvent("user-unsubscribed-from-post", evtDto);
        }


        // Event
        async Task IEventHandler.RequestedForumSubscribersCollected(IEnumerable<string> userIds, int notificationId)
        {
            var evtDto = new RequestedForumSubscribersCollectedEventDto(userIds, notificationId);
            await _publisherService.PublishEvent("requested-forum-subscribers-collected", evtDto); // <- NotificationService listens
        }

        async Task IEventHandler.RequestedPostSubscribersCollected(IEnumerable<string> userIds, int notificationId)
        {
            var evtDto = new RequestedForumSubscribersCollectedEventDto(userIds, notificationId);
            await _publisherService.PublishEvent("requested-post-subscribers-collected", evtDto); // <- NotificationService listens
        }
    }

    public interface IEventHandler
    {
        Task UserSubscribedToForum(string userId, int subscriptionId, int forumId);
        Task FailedToSubscribeUserOnForumPublished(string userId, int forumId);
        Task FailedToSubscribeUserOnPostPublished(string userId, int forumId, int postId);
        Task UserSubscribedToPost(string userId, int subscriptionId, int postId);
        Task UserUnsubscribedFromForum(string userId, int subscriptionId, int forumId);
        Task UserUnsubscribedFromPost(string userId, int subscriptionId, int postId);

        Task RequestedForumSubscribersCollected(IEnumerable<string> userIds, int notificationId);
        Task RequestedPostSubscribersCollected(IEnumerable<string> userIds, int notificationId);
    }
}
