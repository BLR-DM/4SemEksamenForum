using ContentService.Application.Services;
using Dapr.Client;

namespace ContentService.Infrastructure.Services
{
    public class DaprPublisherService : IPublisherService
    {
        private const string Pubsub = "pubsub";
        private readonly DaprClient _daprClient;

        public DaprPublisherService(DaprClient daprClient)
        {
            _daprClient = daprClient;
        }
        async Task IPublisherService.PublishEvent<T>(string topic, T data)
        {
            await _daprClient.PublishEventAsync(Pubsub, topic, data);
        }
    }
}