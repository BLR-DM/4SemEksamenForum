using PointService.Application.Queries.QueryDto;

namespace PointService.Application.Queries
{
    public interface IPointEntryQuery
    {
        Task<UserPointsDto> GetPointsByUserIdAsync(string userId);
        Task<bool> ExistsAsync(string userId, string pointActionId, int contextId, string contextType);
    }
}