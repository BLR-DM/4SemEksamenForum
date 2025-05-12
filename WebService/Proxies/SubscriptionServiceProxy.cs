using System.Net.Http.Json;

namespace WebService.Proxies
{
    public class SubscriptionServiceProxy : ISubscriptionServiceProxy
    {
        private readonly HttpClient _httpClient;

        public SubscriptionServiceProxy(HttpClient httpClient)
        {
            _httpClient = httpClient;
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
    }
}
