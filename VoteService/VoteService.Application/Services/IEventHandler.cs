namespace VoteService.Application.Services
{
    public interface IEventHandler
    {
        Task CommentUpVoteCreated(int commentId, string userId);
        Task CommentDownVoteCreated(int commentId, string userId);
        Task CommentUpVoteRemoved(int commentId, string userId);
        Task PostUpVoteCreated(int postId, string userId);
        Task PostDownVoteCreated(int postId, string userId);
        Task PostUpVoteRemoved(int postId, string userId);
    }
}