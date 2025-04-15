using Microsoft.EntityFrameworkCore;
using PointService.Application.Queries;
using PointService.Application.Queries.QueryDto;

namespace PointService.Infrastructure.Queries
{
    public class PointEntryQuery : IPointEntryQuery
    {
        private readonly PointContext _db;

        public PointEntryQuery(PointContext db)
        {
            _db = db;
        }
        async Task<UserPointsDto> IPointEntryQuery.GetPointsByUserIdAsync(string userId)
        {
            var points = await _db.PointEntries.Where(pe => pe.UserId == userId)
                .Select(pe => pe.Points).SumAsync();

            return new UserPointsDto(points);
        }
    }
}