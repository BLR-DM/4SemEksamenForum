using Microsoft.EntityFrameworkCore;
using PointService.Application.Repositories;
using PointService.Domain.Entities;

namespace PointService.Infrastructure.Repositories
{
    public class PointActionRepository : IPointActionRepository
    {
        private readonly PointContext _db;

        public PointActionRepository(PointContext db)
        {
            _db = db;
        }

        async Task IPointActionRepository.AddAsync(PointAction pointAction)
        {
            _db.PointActions.Add(pointAction);
            await _db.SaveChangesAsync();
        }

        async Task<PointAction> IPointActionRepository.GetAsync(string pointActionId)
        {
            return await _db.PointActions.FirstOrDefaultAsync(pa => pa.Action == pointActionId);
        }

        async Task<bool> IPointActionRepository.UpdateAsync(PointAction pointAction)
        {
            return await _db.SaveChangesAsync() > 0;
        }
    }
}