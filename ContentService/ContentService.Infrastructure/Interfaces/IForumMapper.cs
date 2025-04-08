using ContentService.Application.Queries.QueryDto;
using ContentService.Domain.Entities;

namespace ContentService.Infrastructure.Interfaces
{
    public interface IForumMapper
    {
        ForumDto MapToDto(Forum forum);
        ForumDto MapToDtoWithAll(Forum forum);
    }
}