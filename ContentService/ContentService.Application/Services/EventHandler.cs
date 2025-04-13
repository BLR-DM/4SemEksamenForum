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

        async Task IEventHandler.PostSubmitted(int postId, string content)
        {
            var postSubmittedDto = new PostSubmittedDto(postId, content);
            await _publisherService.PublishEvent("post-submitted", postSubmittedDto);
        }

        async Task IEventHandler.PostPublished(string userId, int forumId, int postId)
        {
            var postPublishedDto = new PostPublishedDto(userId, forumId, postId);
            await _publisherService.PublishEvent("post-published", postPublishedDto);
        }
    }

    public interface IEventHandler
    {
        Task PostSubmitted(int postId, string content);
        Task PostPublished(string userId, int forumId, int postId);
    }
}