using SubscriptionService.Application.EventDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubscriptionService.Application.Services
{
    public class EventHandler : IEventHandler
    {
        private readonly IPublisherService _publisherService;

        public EventHandler(IPublisherService publisherService)
        {
            _publisherService = publisherService;
        }
        async Task IEventHandler.UserSubscribedToForum(string userId, int subscriptionId, int forumId)
        {
            var evtDto = new UserSubscribedToForumEventDto(userId, subscriptionId, forumId);
            await _publisherService.PublishEvent("user-subscribed-to-forum", evtDto);
        }

        async Task IEventHandler.UserSubscribedToPost(string userId, int subscriptionId, int postId)
        {
            var evtDto = new UserSubscribedToPostEventDto(userId, subscriptionId, postId);
            await _publisherService.PublishEvent("user-subscribed-to-post", evtDto);
        }

        async Task IEventHandler.UserUnsubscribedToForum(string userId, int subscriptionId)
        {
            var evtDto = new UserUnSubscribedToForumEventDto(userId, subscriptionId);
            await _publisherService.PublishEvent("user-unsubscribed-to-forum", evtDto);
        }

        async Task IEventHandler.UserUnsubscribedToPost(string userId, int subscriptionId)
        {
            var evtDto = new UserUnSubscribedToPostEventDto(userId, subscriptionId);
            await _publisherService.PublishEvent("user-unsubscribed-to-post", evtDto);
        }
    }

    public interface IEventHandler
    {
        Task UserSubscribedToForum(string userId, int subscriptionId, int forumId);
        Task UserSubscribedToPost(string userId, int subscriptionId, int postId);
        Task UserUnsubscribedToForum(string userId, int subscriptionId);
        Task UserUnsubscribedToPost(string userId, int subscriptionId);
    }
}
