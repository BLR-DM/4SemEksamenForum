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

        async Task IEventHandler.FailedToAddPointsOnForumPublished(string userId, int forumId)
        {
            var evtDto = new FailedToAddPointsOnForumPublishedDto(userId, forumId);
            await _publisherService.PublishEvent("failed-to-add-points-on-forum-published", evtDto);
        }

        async Task IEventHandler.FailedToAddPointsOnPostPublished(string userId, int forumId, int postId)
        {
            var evtDto = new FailedToAddPointsOnPostPublishedDto(userId, forumId, postId);
            await _publisherService.PublishEvent("failed-to-add-points-on-post-published", evtDto);
        }

        async Task IEventHandler.FailedToAddPointsOnCommentPublished(string userId, int forumId, int postId, int commentId)
        {
            var evtDto = new FailedToAddPointsOnCommentPublishedDto(userId, forumId, postId, commentId);
            await _publisherService.PublishEvent("failed-to-add-points-on-comment-published", evtDto);
        }
    }

    public interface IEventHandler
    {
        Task FailedToAddPointsOnForumPublished(string userId, int forumId);
        Task FailedToAddPointsOnPostPublished(string userId, int forumId, int postId);
        Task FailedToAddPointsOnCommentPublished(string userId, int forumId, int postId, int commentId);

    }
}