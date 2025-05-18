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

        async Task<bool> IPointEntryQuery.ExistsAsync(string userId, string pointActionId, int contextId, string contextType)
        {
            try
            {
                var entry = await _db.PointEntries.Where(pa =>
                    pa.UserId == userId && pa.PointActionId == pointActionId && pa.ContextId == contextId &&
                    pa.ContextType == contextType).FirstOrDefaultAsync();

                if (entry == null)
                {
                    return false;
                }

                return true;


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}