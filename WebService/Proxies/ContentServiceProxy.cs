using System.Net.Http.Json;
using WebService.Dtos;

namespace WebService.Proxies
{
    public class ContentServiceProxy : IContentServiceProxy
    {
        private readonly HttpClient _httpClient;

        public ContentServiceProxy(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        async Task<List<ForumDto>> IContentServiceProxy.GetForumsAsync()
        {
            var forumReuqestUri = $"/api/content/forum";

            var forums = await _httpClient.GetFromJsonAsync<List<ForumDto>>(forumReuqestUri);

            if (forums == null)
            {
                throw new Exception("No forums Found");
            }

            return forums;
        }
    }

    public interface IContentServiceProxy
    {
        Task<List<ForumDto>> GetForumsAsync();
    }
}
