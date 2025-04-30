using Dapr.Client;
using NotificationService.Application.Services;

namespace NotificationService.Infrastructure.Services
{
    public class DaprPublisherService(DaprClient daprClient) : IPublisherService
    {
        private const string Pubsub = "pubsub";

        async Task IPublisherService.PublishEvent<T>(string topic, T data)
        {
            await daprClient.PublishEventAsync(Pubsub, topic, data);
        }
    }
}