using ContentService.Application.Queries.QueryDto;
using ContentService.Domain.Entities;
using ContentService.Infrastructure.Interfaces;

namespace ContentService.Infrastructure.Helpers
{
    public class ForumMapper : IForumMapper
    {
        ForumDto IForumMapper.MapToDto(Forum forum)
        {
            return new ForumDto
            {
                Id = forum.Id,
                ForumName = forum.ForumName,
                CreatedDate = forum.CreatedDate.ToString(),
                AppUserId = forum.AppUserId,
                RowVersion = forum.RowVersion
            };
        }

        ForumDto IForumMapper.MapToDtoWithAll(Forum forum)
        {
            var forumDto = new ForumDto
            {
                Id = forum.Id,
                ForumName = forum.ForumName,
                CreatedDate = forum.CreatedDate.ToString(),
                AppUserId = forum.AppUserId,
                RowVersion = forum.RowVersion,
                Posts = forum.Posts.Select(p => new PostDto
                {
                    Id = p.Id.ToString(),
                    Title = p.Title,
                    Content = p.Content,
                    Username = p.Username,
                    AppUserId = p.AppUserId,
                    CreatedDate = p.CreatedDate.ToString(),
                    RowVersion = p.RowVersion,
                    Comments = p.Comments.Select(c => new CommentDto
                    {
                        Id = c.Id,
                        Username = c.Username,
                        Content = c.Content,
                        CreatedDate = c.CreatedDate.ToString(),
                        AppUserId = c.AppUserId,
                        RowVersion = c.RowVersion
                    }).ToList()
                }).ToList()
            };

            return forumDto;
        }
    }
}