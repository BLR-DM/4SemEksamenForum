using System.Net.Http.Json;
using WebService.Dtos;
using WebService.Dtos.CommandDtos;

namespace WebService.Proxies
{
    public class ContentServiceProxy : IContentServiceProxy
    {
        private readonly HttpClient _httpClient;

        public ContentServiceProxy(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        async Task IContentServiceProxy.CreateForum(CreateForumDto dto)
        {
            try
            {
                var uri = $"api/content/forum";

                var response = await _httpClient.PostAsJsonAsync(uri, dto);

                if (!response.IsSuccessStatusCode)
                    throw new Exception("Failed to create forum");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new Exception("Failed to create forum");
            }
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
                return new List<ForumDto>();
            }
        }
    }

    public interface IContentServiceProxy
    {
        Task<List<ForumDto>> GetForumsAsync();
        Task CreateForum(CreateForumDto dto);
    }
}
