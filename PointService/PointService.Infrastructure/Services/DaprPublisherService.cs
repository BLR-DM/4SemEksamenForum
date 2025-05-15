using PointService.Application.Services;
using Dapr.Client;

namespace PointService.Infrastructure.Services
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