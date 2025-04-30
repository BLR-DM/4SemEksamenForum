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


        async Task IEventHandler.ForumSubscribersRequested(int forumId, int postId)
        {
            var evtDto = new ForumSubscribersRequestedEventDto(forumId, postId);
            await _publisherService.PublishEvent("forum-subscribers-requested", evtDto);
        }

        Task IEventHandler.ForumSubscribersNotified(IEnumerable<string> userIds, int forumId, int postId)
        {
            throw new NotImplementedException();
        }
    }

    public interface IEventHandler
    {
        Task ForumSubscribersRequested(int forumId, int postId);
        Task ForumSubscribersNotified(IEnumerable<string> userIds, int forumId, int postId);
    }
}