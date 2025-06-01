using System.Net.Http.Json;
using WebService.Dtos;
using WebService.Dtos.CommandDtos;

namespace WebService.Proxies
{
    public class PointServiceProxy : IPointServiceProxy
    {
        private readonly HttpClient _httpClient;

        public PointServiceProxy(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        async Task<List<PointActionDto>> IPointServiceProxy.GetPointActions()
        {
            try
            {
                var uri = $"point/api/pointactions";

                var pointActions = await _httpClient.GetFromJsonAsync<List<PointActionDto>>(uri);

                if (pointActions == null)
                {
                    throw new Exception("Failed to retrieve pointactions");
                }

                return pointActions;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("Failed to retrieve pointactions");
            }
        }

        async Task<UserPointsDto> IPointServiceProxy.GetPointsByUserId(string userId)
        {
            try
            {
                var uri = $"point/api/User/{userId}/Points";

                var userPointsDto = await _httpClient.GetFromJsonAsync<UserPointsDto>(uri);

                if (userPointsDto == null)
                {
                    throw new Exception("Failed to retrieve points");
                }

                return userPointsDto;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("Failed to retrieve points");
            }

        }

        async Task<bool> IPointServiceProxy.UpdatePointAction(UpdatePointActionDto dto)
        {
            try
            {
                var uri = "point/api/PointAction";
                var response = await _httpClient.PutAsJsonAsync(uri, dto);
                if (!response.IsSuccessStatusCode)
                {
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("Failed to update point action");
            }
        }
    }

    public interface IPointServiceProxy
    {
        Task<UserPointsDto> GetPointsByUserId(string userId);
        Task<bool> UpdatePointAction(UpdatePointActionDto dto);
        Task<List<PointActionDto>> GetPointActions();
    }
}
