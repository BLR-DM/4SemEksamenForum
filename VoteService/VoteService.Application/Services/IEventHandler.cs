namespace VoteService.Application.Services
{
    public interface IEventHandler
    {
        Task CommentVoteCreated(string commentId, string userId, bool voteType);
        Task CommentVoteUpdated(string commentId, string userId, bool voteType);
        Task CommentVoteDeleted(string commentId, string userId, bool voteType);
        Task PostVoteCreated(string postId, string userId, bool voteType);
        Task PostVoteUpdated(string postId, string userId, bool voteType);
        Task PostVoteDeleted(string postId, string userId, bool voteType);
    }
}