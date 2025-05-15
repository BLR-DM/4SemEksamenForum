using PointService.Application.EventDto;

namespace PointService.Application.Services
{
    public class EventHandler : IEventHandler
    {
        private readonly IPublisherService _publisherService;

        public EventHandler(IPublisherService publisherService)
        {
            _publisherService = publisherService;
        }

        Task IEventHandler.FailedToAddPointsOnForumPublished(int forumId)
        {
            var evtDto = new FailedToAddPointsOnForumPublishedDto(forumId);
            await _publisherService.PublishEvent("failed-to-add-points-on-forum-published", evtDto);
        }

        async Task IEventHandler.FailedToAddPointsOnPostPublished(int forumId, int postId)
        {
            var evtDto = new FailedToAddPointsOnPostPublishedDto(forumId, postId);
            await _publisherService.PublishEvent("failed-to-add-points-on-post-published", evtDto);
        }
    }

    public interface IEventHandler
    {
        Task FailedToAddPointsOnForumPublished(int forumId);
        Task FailedToAddPointsOnPostPublished(int forumId, int postId);

    }
}