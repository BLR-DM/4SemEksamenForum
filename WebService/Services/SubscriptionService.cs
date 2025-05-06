using WebService.Proxies;

namespace WebService.Services
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly ISubscriptionServiceProxy _subscriptionServiceProxy;

        public SubscriptionService(ISubscriptionServiceProxy subscriptionServiceProxy)
        {
            _subscriptionServiceProxy = subscriptionServiceProxy;
        }

        async Task<List<int>> ISubscriptionService.GetSubscribedForumIds(string userId)
        {
            var subscribedForumIds = await _subscriptionServiceProxy.GetSubscribedForumIds(userId);

            return subscribedForumIds;
        }
    }

    public interface ISubscriptionService
    {
        Task<List<int>> GetSubscribedForumIds(string userId);
    }
}

