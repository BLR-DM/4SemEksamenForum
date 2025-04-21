
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

        async Task IEventHandler.CommentVoteCreated(string commentId, string userId, bool voteType)
        {
            var commentVoteCreatedDto = new CommentVoteEventDto(commentId, userId, voteType);
            await _publisherService.PublishEvent("commentvote-created", commentVoteCreatedDto);
        }

        async Task IEventHandler.CommentVoteDeleted(string commentId, string userId, bool voteType)
        {
            var commentVoteDeletedDto = new CommentVoteEventDto(commentId, userId, voteType);
            await _publisherService.PublishEvent("commentvote-deleted", commentVoteDeletedDto);
        }

        async Task IEventHandler.CommentVoteUpdated(string commentId, string userId, bool voteType)
        {
            var commentVoteUpdatedDto = new CommentVoteEventDto(commentId, userId, voteType);
            await _publisherService.PublishEvent("commentvote-updated", commentVoteUpdatedDto);
        }

        async Task IEventHandler.PostVoteCreated(string postId, string userId, bool voteType)
        {
            var postVoteCreatedDto = new PostVoteEventDto(postId, userId, voteType);
            await _publisherService.PublishEvent("postvote-created", postVoteCreatedDto);
        }

        async Task IEventHandler.PostVoteDeleted(string postId, string userId, bool voteType)
        {
            var postVoteDeletedDto = new PostVoteEventDto(postId, userId, voteType);
            await _publisherService.PublishEvent("postvote-deleted", postVoteDeletedDto);
        }

        async Task IEventHandler.PostVoteUpdated(string postId, string userId, bool voteType)
        {
            var postVoteUpdatedDto = new PostVoteEventDto(postId, userId, voteType);
            await _publisherService.PublishEvent("postvote-updated", postVoteUpdatedDto);
        }
    }
}