using WebService.Proxies;

namespace WebService.Services
{
    public class PointService : IPointService
    {
        private readonly IPointServiceProxy _pointServiceProxy;

        public PointService(IPointServiceProxy pointServiceProxy)
        {
            _pointServiceProxy = pointServiceProxy;
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
    }

    public interface IPointService
    {
        Task<int> GetPointsByUserId(string userId);
    }
}
