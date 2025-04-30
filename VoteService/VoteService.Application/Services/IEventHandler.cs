namespace VoteService.Application.Services
{
    public interface IEventHandler
    {
        Task CommentUpVoteCreated(string commentId, string userId);
        Task CommentDownVoteCreated(string commentId, string userId);
        Task CommentUpVoteRemoved(string commentId, string userId);
        Task PostUpVoteCreated(string postId, string userId);
        Task PostDownVoteCreated(string postId, string userId);
        Task PostUpVoteRemoved(string postId, string userId);
    }
}