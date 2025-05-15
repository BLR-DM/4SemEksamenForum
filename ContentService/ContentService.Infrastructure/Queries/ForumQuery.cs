using ContentService.Application.Queries;
using ContentService.Application.Queries.QueryDto;
using ContentService.Domain.Entities;
using ContentService.Domain.Enums;
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
                .FirstAsync(f => f.Id == forumId && f.Status != Status.Rejected);

            return _forumMapper.MapToDto(forum);
        }

        async Task<IEnumerable<ForumDto>> IForumQuery.GetForumsAsync()
        {
            var forums = await _db.Forums.AsNoTracking()
                .Where(f => f.Status != Status.Rejected)
                .ToListAsync();

            return forums.Select(forum => _forumMapper.MapToDto(forum));
        }

        async Task<ForumDto> IForumQuery.GetForumWithPostsAsync(int forumId)
        {
            var forum = await _db.Forums.AsNoTracking()
                .Include(f => f.Posts
                    .Where(p => p.Status != Status.Rejected))
                .ThenInclude(p => p.Comments
                    .Where(c => c.Status != Status.Rejected))
                .FirstOrDefaultAsync(f => f.Id == forumId && f.Status != Status.Rejected);

            return _forumMapper.MapToDtoWithAll(forum);
        }

        async Task<List<ForumDto>> IForumQuery.GetForumsWithPostsAsync()
        {
            var forum = await _db.Forums.AsNoTracking()
                .Include(f => f.Posts
                    .Where(p => p.Status != Status.Rejected))
                .ToListAsync();

            var forumDtos = forum.Select(f => _forumMapper.MapToDtoWithPost(f)).ToList();

            return forumDtos;
        }

        async Task<ForumDto> IForumQuery.GetForumByNameWithPostsAsync(string forumName)
        {
            var forum = await _db.Forums.AsNoTracking()
                .Include(f => f.Posts
                    .Where(p => p.Status != Status.Rejected))
                .ThenInclude(p => p.Comments
                    .Where(c => c.Status != Status.Rejected))
                .FirstAsync(f => f.ForumName.Equals(forumName) && f.Status != Status.Rejected);

            return _forumMapper.MapToDtoWithAll(forum);
        }

        async Task<ForumDto> IForumQuery.GetForumWithSinglePostAsync(int forumId, int postId)
        {
            var forum = await _db.Forums.AsNoTracking()
                .Include(f => f.Posts
                    .Where(p => p.Id == postId && p.Status != Status.Rejected))
                .ThenInclude(p => p.Comments
                    .Where(c => c.Status != Status.Rejected))
                .FirstOrDefaultAsync(f => f.Id == forumId && f.Status != Status.Rejected);


            return _forumMapper.MapToDtoWithAll(forum);
        }

        async Task<ForumDto> IForumQuery.GetForumByNameWithSinglePostAsync(string forumName, int postId)
        {
            var forum = await _db.Forums.AsNoTracking()
                .Include(f => f.Posts
                    .Where(p => p.Id == postId && p.Status != Status.Rejected))
                .ThenInclude(p => p.Comments
                    .Where(c => c.Status != Status.Rejected))
                .FirstAsync(f => f.ForumName.Equals(forumName) && f.Status != Status.Rejected);

            return _forumMapper.MapToDtoWithAll(forum);
        }

    }
}