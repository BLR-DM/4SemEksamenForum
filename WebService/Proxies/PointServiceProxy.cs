using System.Net.Http.Json;
using WebService.Dtos;

namespace WebService.Proxies
{
    public class PointServiceProxy : IPointServiceProxy
    {
        private readonly HttpClient _httpClient;

        public PointServiceProxy(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        async Task<UserPointsDto> IPointServiceProxy.GetPointsByUserId(string userId)
        {
            try
            {
                var uri = $"point/User/{userId}/Points";

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
    }

    public interface IPointServiceProxy
    {
        Task<UserPointsDto> GetPointsByUserId(string userId);
    }
}
