using ContentService.Application.Queries;
using ContentService.Application.Queries.QueryDto;
using ContentService.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ContentService.Infrastructure.Queries
{
    public class ForumQuery : IForumQuery
    {
        private readonly ContentContext _db;
        private readonly IForumMapper _forumMapper;

        public ForumQuery(ContentContext db, IForumMapper forumMapper)
        {
            _db = db;
            _forumMapper = forumMapper;
        }
        async Task<ForumDto> IForumQuery.GetForumAsync(int forumId)
        {
            var forum = await _db.Forums.AsNoTracking()
                .FirstAsync(f => f.Id == forumId);
            return _forumMapper.MapToDto(forum);
        }

        async Task<IEnumerable<ForumDto>> IForumQuery.GetForumsAsync()
        {
            var forums = await _db.Forums.AsNoTracking().ToListAsync();
            return forums.Select(forum => _forumMapper.MapToDto(forum));
        }

        async Task<ForumDto> IForumQuery.GetForumWithPostsAsync(int forumId)
        {
            var forum = await _db.Forums.AsNoTracking()
                .Include(f => f.Posts)
                .ThenInclude(p => p.Comments)
                .FirstOrDefaultAsync(f => f.Id == forumId);

            return _forumMapper.MapToDtoWithAll(forum);
        }

        async Task<ForumDto> IForumQuery.GetForumWithSinglePostAsync(int forumId, int postId)
        {
            var forum = await _db.Forums.AsNoTracking()
                .Where(f => f.Id == forumId)
                .Include(f => f.Posts.Where(p => p.Id == postId))
                .ThenInclude(p => p.Comments)
                .FirstOrDefaultAsync();

            return _forumMapper.MapToDtoWithAll(forum);
        }
    }
}