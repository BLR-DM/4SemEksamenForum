
using VoteService.Application.Services.EventDto;

namespace VoteService.Application.Services
{
    public class EventHandler : IEventHandler
    {
        private readonly IPublisherService _publisherService;

        public EventHandler(IPublisherService publisherService)
        {
            _publisherService = publisherService;
        }

        async Task IEventHandler.CommentDownVoteCreated(string commentId, string userId)
        {
            var commentVoteCreatedDto = new CommentVoteEventDto(commentId, userId);
            await _publisherService.PublishEvent("comment-downvote-created", commentVoteCreatedDto);
        }

        async Task IEventHandler.CommentUpVoteCreated(string commentId, string userId)
        {
            var commentVoteCreatedDto = new CommentVoteEventDto(commentId, userId);
            await _publisherService.PublishEvent("comment-upvote-created", commentVoteCreatedDto);
        }

        async Task IEventHandler.CommentUpVoteRemoved(string commentId, string userId)
        {
            var commentVoteDeletedDto = new CommentVoteEventDto(commentId, userId);
            await _publisherService.PublishEvent("comment-upvote-deleted", commentVoteDeletedDto);
        }

        async Task IEventHandler.PostDownVoteCreated(string postId, string userId)
        {
            var postVoteCreatedDto = new PostVoteEventDto(postId, userId);
            await _publisherService.PublishEvent("post-downvote-created", postVoteCreatedDto);
        }

        async Task IEventHandler.PostUpVoteCreated(string postId, string userId)
        {
            var postVoteCreatedDto = new PostVoteEventDto(postId, userId);
            await _publisherService.PublishEvent("post-upvote-created", postVoteCreatedDto);
        }

        async Task IEventHandler.PostUpVoteRemoved(string postId, string userId)
        {
            var postVoteDeletedDto = new PostVoteEventDto(postId, userId);
            await _publisherService.PublishEvent("post-upvote-deleted", postVoteDeletedDto);
        }

    }
}