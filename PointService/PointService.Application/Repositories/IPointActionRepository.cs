using PointService.Domain.Entities;

namespace PointService.Application.Repositories
{
    public interface IPointActionRepository
    {
        Task AddAsync(PointAction pointAction);
        Task<PointAction> GetAsync(string pointActionId);
    }
}