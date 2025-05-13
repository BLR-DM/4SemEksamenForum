using System.Net.Http.Json;
using WebService.Dtos;

namespace WebService.Proxies
{
    public class NotificationServiceProxy : INotificationServiceProxy
    {
        private readonly HttpClient _httpClient;

        public NotificationServiceProxy(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        async Task<List<NotificationDto>> INotificationServiceProxy.GetNotificationsByUserId(string userId)
        {
            try
            {
                var requestUri = $"notification/{userId}/notifications";

                var notifications = await _httpClient.GetFromJsonAsync<List<NotificationDto>>(requestUri);

                return notifications ?? new List<NotificationDto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }

    public interface INotificationServiceProxy
    {
        Task<List<NotificationDto>> GetNotificationsByUserId(string userId);

    }
}