using WebService.Dtos;
using WebService.Dtos.CommandDtos;
using WebService.Helpers;
using WebService.Proxies;
using WebService.Views;

namespace WebService.Services
{
    public class PointService : IPointService
    {
        private readonly IPointServiceProxy _pointServiceProxy;

        public PointService(IPointServiceProxy pointServiceProxy)
        {
            _pointServiceProxy = pointServiceProxy;
        }

        async Task<List<PointActionView>> IPointService.GetPointActions()
        {
            try
            {
                var pointActions = await _pointServiceProxy.GetPointActions();

                var pointActionViews = pointActions.Select(MapDtoToView.MapPointActionToView).ToList();

                return pointActionViews;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("Failed to retrieve pointactions");
            }
        }

        async Task<int> IPointService.GetPointsByUserId(string userId)
        {
            try
            {
                var userPointsDto = await _pointServiceProxy.GetPointsByUserId(userId);

                return userPointsDto.Points;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new Exception("Failed to retrieve points");
            }
        }

        async Task<bool> IPointService.UpdatePointAction(UpdatePointActionDto dto)
        {
            try
            {
                return await _pointServiceProxy.UpdatePointAction(dto);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }

    public interface IPointService
    {
        Task<int> GetPointsByUserId(string userId);
        Task<bool> UpdatePointAction(UpdatePointActionDto dto);
        Task<List<PointActionView>> GetPointActions();
    }
}
