using PointService.Application.Queries.QueryDto;

namespace PointService.Application.Queries
{
    public interface IPointActionQuery
    {
        Task<List<PointActionDto>> GetAllPointActionsAsync();
    }
}