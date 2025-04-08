using ContentService.Domain.Entities;

namespace ContentService.Application
{
    public interface IForumRepository
    {
        Task AddForumAsync(Forum forum);
        void UpdateForumAsync(Forum forum, uint rowVersion);
        Task<Forum> GetForumAsync(int id);
        Task<Forum> GetForumWithSinglePostAsync(int forumId, int postId);
        void DeleteForum(Forum forum, uint rowVersion);
        void UpdatePost(Post post, uint rowVersion);
        void DeletePost(Post post, uint rowVersion);
        void UpdateComment(Comment comment, uint rowVersion);
        void DeleteComment(Comment comment, uint rowVersion);
        Task SaveChangesAsync();
    }
}