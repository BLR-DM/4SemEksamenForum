using WebService.Dtos.CommandDtos;
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

        async Task ISubscriptionService.CreateSubscription(CreateSubDto dto)
        {
            try
            {
                await _subscriptionServiceProxy.CreateSubscription(dto);
            }
            catch (Exception)
            {
                throw;
            }
        }

        async Task ISubscriptionService.DeleteSubscription(int forumId)
        {
            try
            {
                await _subscriptionServiceProxy.DeleteSubscription(forumId);
            }
            catch (Exception)
            {
                throw;
            }
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
        Task CreateSubscription(CreateSubDto dto);
        Task DeleteSubscription(int forumId);
    }
}

