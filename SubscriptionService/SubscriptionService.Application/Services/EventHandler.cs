
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

        async Task IEventHandler.FailedToSubscribeUserOnForumCreation(string userId, int forumId)
        {
            var evtDto = new FailedToSubscribeUserToForumEventDto(userId, forumId);
            await _publisherService.PublishEvent("user-subscribed-to-forum-on-creation-failed", evtDto);
        }

        async Task IEventHandler.UserUnsubscribedFromForum(string userId, int subscriptionId)
        {
            var evtDto = new UserUnSubscribedToForumEventDto(userId, subscriptionId);
            await _publisherService.PublishEvent("user-unsubscribed-from-forum", evtDto);
        }

        // Post
        async Task IEventHandler.UserSubscribedToPost(string userId, int subscriptionId, int postId)
        {
            var evtDto = new UserSubscribedToPostEventDto(userId, subscriptionId, postId);
            await _publisherService.PublishEvent("user-subscribed-to-post", evtDto);
        }

        async Task IEventHandler.UserUnsubscribedFromPost(string userId, int subscriptionId)
        {
            var evtDto = new UserUnSubscribedToPostEventDto(userId, subscriptionId);
            await _publisherService.PublishEvent("user-unsubscribed-from-post", evtDto);
        }


        // Event
        async Task IEventHandler.RequestedForumSubscribersCollected(IEnumerable<string> userIds, int forumId, int postId)
        {
            var evtDto = new RequestedForumSubscribersCollectedEventDto(userIds, forumId, postId);
            await _publisherService.PublishEvent("requested-forum-subscribers-collected", evtDto); // <- NotificationService listens
        }

    }

    public interface IEventHandler
    {
        Task UserSubscribedToForum(string userId, int subscriptionId, int forumId);
        Task FailedToSubscribeUserOnForumCreation(string userId, int forumId);
        Task UserSubscribedToPost(string userId, int subscriptionId, int postId);
        Task UserUnsubscribedFromForum(string userId, int subscriptionId);
        Task UserUnsubscribedFromPost(string userId, int subscriptionId);

        Task RequestedForumSubscribersCollected(IEnumerable<string> userIds, int forumId, int postId);
    }
}
