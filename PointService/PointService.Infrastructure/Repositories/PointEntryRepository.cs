using PointService.Application.Repositories;
using PointService.Domain.Entities;

namespace PointService.Infrastructure.Repositories
{
    public class PointEntryRepository : IPointEntryRepository
    {
        private readonly PointContext _db;

        public PointEntryRepository(PointContext db)
        {
            _db = db;
        }

        async Task IPointEntryRepository.AddAsync(PointEntry pointEntry)
        {
            _db.PointEntries.Add(pointEntry);
            await _db.SaveChangesAsync();
        }
    }
}