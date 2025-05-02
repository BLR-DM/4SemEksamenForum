using System.Net.Http.Json;
using WebService.Dtos;

namespace WebService.Proxies
{
    public class ApiProxy : IApiProxy
    {
        private readonly HttpClient _httpClient;

        public ApiProxy(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        async Task<ForumDto> IApiProxy.GetForumWithPosts(string forumName)
        {
            var forumRequestUri = $"/api/forum/{forumName}/posts";

            var forum = await _httpClient.GetFromJsonAsync<ForumDto>(forumRequestUri);

            if (forum == null)
            {
                throw new Exception("Forum not found");
            }

            return forum;
        }
    }

    public interface IApiProxy
    {
        Task<ForumDto> GetForumWithPosts(string forumName);
    }
}