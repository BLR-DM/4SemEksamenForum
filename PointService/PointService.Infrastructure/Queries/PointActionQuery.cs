using Microsoft.EntityFrameworkCore;
using PointService.Application.Queries;
using PointService.Application.Queries.QueryDto;

namespace PointService.Infrastructure.Queries
{
    public class PointActionQuery : IPointActionQuery
    {
        private readonly PointContext _db;
        public PointActionQuery(PointContext db)
        {
            _db = db;
        }

        async Task<List<PointActionDto>> IPointActionQuery.GetAllPointActionsAsync()
        { 
            var pointActions = await _db.PointActions.ToListAsync();

            var pointActionDtos = pointActions.Select(pa => new PointActionDto
            {
                Action = pa.Action,
                Points = pa.Points
            }).ToList();

            return pointActionDtos;
        }
    }
}
