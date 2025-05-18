using ContentService.Domain.Entities;

namespace ContentService.Application
{
    public interface IForumRepository
    {
        Task AddForumAsync(Forum forum);
        void UpdateForumAsync(Forum forum, uint rowVersion);
        Task<Forum> GetForumOnlyAsync(int forumId);
        Task<Forum> GetForumWithPostsAsync(int forumId);
        Task<Forum> GetForumWithAllAsync(int forumId);
        Task<IEnumerable<Forum>> GetForumsAsync();
        Task<Forum> GetForumWithSinglePostAsync(int forumId, int postId);
        void DeleteForum(Forum forum);
        void UpdatePost(Post post, uint rowVersion);
        void DeletePost(Post post);
        void DeletePosts(IEnumerable<Post> posts);
        void UpdateComment(Comment comment, uint rowVersion);
        void DeleteComment(Comment comment);
        void DeleteComments(IEnumerable<Comment> comments);
        Task SaveChangesAsync();
    }
}