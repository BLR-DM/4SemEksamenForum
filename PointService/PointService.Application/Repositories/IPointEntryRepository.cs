using PointService.Domain.Entities;

namespace PointService.Application.Repositories
{
    public interface IPointEntryRepository
    {
        Task AddAsync(PointEntry pointEntry);
    }
}