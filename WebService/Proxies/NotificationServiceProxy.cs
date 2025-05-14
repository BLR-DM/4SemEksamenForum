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

        async Task INotificationServiceProxy.MarkNotificationAsRead(int notificationId)
        {
            try
            {
                var requestUri = $"notification/notifications/{notificationId}/read";

                var request = new HttpRequestMessage(HttpMethod.Patch, requestUri);
                var response = await _httpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                    throw new Exception("Failed to mark notification as read");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new Exception("Failed to mark notification as read");
            }
        }
    }

    public interface INotificationServiceProxy
    {
        Task<List<NotificationDto>> GetNotificationsByUserId(string userId);
        Task MarkNotificationAsRead(int notificationId);

    }
}