using ContentService.Application.EventDto;

namespace ContentService.Application.Services
{
    public class EventHandler : IEventHandler
    {
        private readonly IPublisherService _publisherService;

        public EventHandler(IPublisherService publisherService)
        {
            _publisherService = publisherService;
        }

        async Task IEventHandler.ContentSubmitted(string contentId, string content)
        {
            var contentSubmittedDto = new ContentSubmittedDto(contentId, content);
            await _publisherService.PublishEvent("content-submitted", contentSubmittedDto);
        }

        async Task IEventHandler.ForumPublished(string userId, int forumId)
        {
            var forumPublishedDto = new ForumPublishedDto(userId, forumId);
            await _publisherService.PublishEvent("forum-published", forumPublishedDto);
        }
        //async Task IEventHandler.PostSubmitted(int postId, string content)
        //{
        //    var postSubmittedDto = new PostSubmittedDto(postId, content);
        //    await _publisherService.PublishEvent("post-submitted", postSubmittedDto);
        //}

        async Task IEventHandler.PostPublished(string userId, int forumId, int postId)
        {
            var postPublishedDto = new PostPublishedDto(userId, forumId, postId);
            await _publisherService.PublishEvent("post-published", postPublishedDto);
        }

        async Task IEventHandler.CommentPublished(string userId, int forumId, int postId, int commentId)
        {
            var commentPublishedDto = new CommentPublishedDto(userId, forumId, postId, commentId);
            await _publisherService.PublishEvent("comment-published", commentPublishedDto);
        }
    }

    public interface IEventHandler
    {
        Task ContentSubmitted(string contentId, string content);
        Task ForumPublished(string userId, int forumId);
        //Task PostSubmitted(int postId, string content);
        Task PostPublished(string userId, int forumId, int postId);
        Task CommentPublished(string userId, int forumId, int postId, int commentId);
    }
}