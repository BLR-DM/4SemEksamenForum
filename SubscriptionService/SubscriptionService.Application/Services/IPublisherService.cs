namespace SubscriptionService.Application.Services
{
    public interface IPublisherService
    {
        Task PublishEvent<T>(string topic, T data);
    }
}