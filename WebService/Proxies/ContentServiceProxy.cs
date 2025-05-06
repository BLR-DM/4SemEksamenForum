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
            try
            {
                var forumReuqestUri = $"/api/content/forum";

                var forums = await _httpClient.GetFromJsonAsync<List<ForumDto>>(forumReuqestUri);

                if (forums == null)
                {
                    throw new Exception("No forums Found");
                }

                return forums;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
    }

    public interface IContentServiceProxy
    {
        Task<List<ForumDto>> GetForumsAsync();
    }
}
