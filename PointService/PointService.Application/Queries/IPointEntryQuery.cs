using PointService.Application.Queries.QueryDto;

namespace PointService.Application.Queries
{
    public interface IPointEntryQuery
    {
        Task<UserPointsDto> GetPointsByUserIdAsync(string userId); 
    }
}