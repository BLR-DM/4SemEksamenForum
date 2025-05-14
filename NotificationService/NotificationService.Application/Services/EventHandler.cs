using NotificationService.Application.EventDtos;

namespace NotificationService.Application.Services
{
    public class EventHandler : IEventHandler
    {
        private readonly IPublisherService _publisherService;

        public EventHandler(IPublisherService publisherService)
        {
            _publisherService = publisherService;
        }


        async Task IEventHandler.ForumSubscribersRequested(int notificationId, int forumId)
        {
            var evtDto = new ForumSubscribersRequestedEventDto(notificationId, forumId);
            await _publisherService.PublishEvent("forum-subscribers-requested", evtDto);
        }

        Task IEventHandler.ForumSubscribersNotified(IEnumerable<string> userIds, int forumId, int postId)
        {
            throw new NotImplementedException(); 
        }
    }

    public interface IEventHandler
    {
        Task ForumSubscribersRequested(int notificationId, int forumId);
        Task ForumSubscribersNotified(IEnumerable<string> userIds, int forumId, int postId);
    }
}