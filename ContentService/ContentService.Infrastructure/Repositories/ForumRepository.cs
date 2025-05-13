using ContentService.Application;
using ContentService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ContentService.Infrastructure.Repositories
{
    public class ForumRepository : IForumRepository
    {
        private readonly ContentContext _db;

        public ForumRepository(ContentContext db)
        {
            _db = db;
        }

        async Task IForumRepository.AddForumAsync(Forum forum)
        {
            await _db.Forums.AddAsync(forum);
        }

        void IForumRepository.UpdateForumAsync(Forum forum, uint rowVersion)
        {
            _db.Entry(forum).Property(nameof(forum.RowVersion)).OriginalValue = rowVersion;
        }

        void IForumRepository.DeleteForum(Forum forum)
        {
            _db.Forums.Remove(forum);
        }

        void IForumRepository.UpdatePost(Post post, uint rowVersion)
        {
            _db.Entry(post).Property(nameof(post.RowVersion)).OriginalValue = rowVersion;
        }

        void IForumRepository.DeletePost(Post post)
        {
            _db.Posts.Remove(post);
        }

        void IForumRepository.UpdateComment(Comment comment, uint rowVersion)
        {
            _db.Entry(comment).Property(nameof(comment.RowVersion)).OriginalValue = rowVersion;
        }

        void IForumRepository.DeleteComment(Comment comment, uint rowVersion)
        {
            _db.Entry(comment).Property(nameof(comment.RowVersion)).OriginalValue = rowVersion;
            _db.Comments.Remove(comment);
        }

        async Task<Forum> IForumRepository.GetForumOnlyAsync(int forumId)
        {
            return await _db.Forums.FirstAsync(forum => forum.Id == forumId);

        }

        async Task<Forum> IForumRepository.GetForumAsync(int forumId)
        {
            return await _db.Forums
                .Include(f => f.Posts)
                .FirstAsync(forum => forum.Id == forumId);
        }

        async Task<Forum> IForumRepository.GetForumWithSinglePostAsync(int forumId, int postId)
        {
            return await _db.Forums
                .Include(f => f.Posts
                    .Where(p => p.Id == postId))
                    .ThenInclude(p => p.Comments)
                .FirstAsync(f => f.Id == forumId);
        }
        
        async Task IForumRepository.SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}