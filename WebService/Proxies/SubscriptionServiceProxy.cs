namespace WebService.Proxies
{
    public class SubscriptionServiceProxy
    {
        private readonly HttpClient _httpClient;

        public SubscriptionServiceProxy(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

    }
}
