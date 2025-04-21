using ContentService.Application.EventDto.CommentEventDto;
using ContentService.Application.EventDto.ForumEventDto;
using ContentService.Application.EventDto.PostEventDto;

namespace ContentService.Application.Services
{
    public class EventHandler : IEventHandler
    {
        private readonly IPublisherService _publisherService;

        public EventHandler(IPublisherService publisherService)
        {
            _publisherService = publisherService;
        }

        // Submit
        async Task IEventHandler.ForumSubmitted(string forumId, string content)
        {
            var forumSubmittedDto = new ForumSubmittedDto(forumId, content);
            await _publisherService.PublishEvent("forum-submitted", forumSubmittedDto);
        }

        async Task IEventHandler.PostSubmitted(string postId, string content)
        {
            var postSubmittedDto = new PostSubmittedDto(postId, content);
            await _publisherService.PublishEvent("post-submitted", postSubmittedDto);
        }

        async Task IEventHandler.CommentSubmitted(string commentId, string content)
        {
            var commentSubmittedDto = new CommentSubmittedDto(commentId, content);
            await _publisherService.PublishEvent("comment-submitted", commentSubmittedDto);
        }

        // Publish
        async Task IEventHandler.ForumPublished(string userId, int forumId)
        {
            var forumPublishedDto = new ForumPublishedDto(userId, forumId);
            await _publisherService.PublishEvent("forum-published", forumPublishedDto);
        }

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

        // Reject
        async Task IEventHandler.ForumRejected(string userId, int forumId)
        {
            var forumRejectedDto = new ForumRejectedDto(userId, forumId);
            await _publisherService.PublishEvent("forum-rejected", forumRejectedDto);
        }

        async Task IEventHandler.PostRejected(string userId, int forumId, int postId)
        {
            var postRejectedDto = new PostRejectedDto(userId, forumId, postId);
            await _publisherService.PublishEvent("post-rejected", postRejectedDto);
        }

        async Task IEventHandler.CommentRejected(string userId, int forumId, int postId, int commentId)
        {
            var commentRejectedDto = new CommentRejectedDto(userId, forumId, postId, commentId);
            await _publisherService.PublishEvent("comment-rejected", commentRejectedDto);
        }
    }

    public interface IEventHandler
    {
        Task ForumSubmitted(string forumId, string content);
        Task PostSubmitted(string postId, string content);
        Task CommentSubmitted(string commentId, string content);
        Task ForumPublished(string userId, int forumId);
        Task PostPublished(string userId, int forumId, int postId);
        Task CommentPublished(string userId, int forumId, int postId, int commentId);
        Task ForumRejected(string userId, int forumId);
        Task PostRejected(string userId, int forumId, int postId);
        Task CommentRejected(string userId, int forumId, int postId, int commentId);
    }
}