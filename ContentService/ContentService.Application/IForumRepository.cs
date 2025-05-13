using ContentService.Domain.Entities;

namespace ContentService.Application
{
    public interface IForumRepository
    {
        Task AddForumAsync(Forum forum);
        void UpdateForumAsync(Forum forum, uint rowVersion);
        Task<Forum> GetForumOnlyAsync(int forumId);
        Task<Forum> GetForumAsync(int id);
        Task<Forum> GetForumWithSinglePostAsync(int forumId, int postId);
        void DeleteForum(Forum forum);
        void UpdatePost(Post post, uint rowVersion);
        void DeletePost(Post post);
        void UpdateComment(Comment comment, uint rowVersion);
        void DeleteComment(Comment comment);
        Task SaveChangesAsync();
    }
}