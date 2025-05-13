using System.Net.Http.Json;
using WebService.Dtos.CommandDtos;

namespace WebService.Proxies
{
    public class SubscriptionServiceProxy : ISubscriptionServiceProxy
    {
        private readonly HttpClient _httpClient;

        public SubscriptionServiceProxy(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        async Task ISubscriptionServiceProxy.CreateSubscription(CreateSubDto dto)
        {
            try
            {
                var uri = $"subscription/Forums/{dto.ForumId}/Subscriptions/";

                var response = await _httpClient.PostAsJsonAsync(uri, dto);

                if (!response.IsSuccessStatusCode)
                    throw new Exception("Failed to create subscription");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new Exception("Failed to create subscription");
            }

        }

        async Task ISubscriptionServiceProxy.DeleteSubscription(int forumId)
        {
            try
            {
                var uri = $"subscription/Forums/{forumId}/Subscriptions";

                var response = await _httpClient.DeleteAsync(uri);

                if (!response.IsSuccessStatusCode)
                    throw new Exception("Failed to delete subscription");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new Exception("Failed to delete subscription");
            }
        }

        async Task<List<int>> ISubscriptionServiceProxy.GetSubscribedForumIds(string userId)
        {
            var subscriptionRequestUri = $"subscription/Users/{userId}/Forums/Subscriptions/";

            var forumIds = await _httpClient.GetFromJsonAsync<List<int>>(subscriptionRequestUri);

            if (forumIds == null)
            {
                throw new Exception("No subscriptions");
            }

            return forumIds;
        }
    }

    public interface ISubscriptionServiceProxy
    {
        Task<List<int>> GetSubscribedForumIds(string userId);
        Task CreateSubscription(CreateSubDto dto);
        Task DeleteSubscription(int forumId);

    }
}
