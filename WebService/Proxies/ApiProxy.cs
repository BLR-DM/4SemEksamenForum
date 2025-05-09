using System.Net.Http.Json;
using WebService.Dtos;
using WebService.Pages;

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
            var forumRequestUri = $"/api/forums/{forumName}/posts";

            var forum = await _httpClient.GetFromJsonAsync<ForumDto>(forumRequestUri);

            if (forum == null)
            {
                throw new Exception("Forum not found");
            }

            return forum;
        }

        async Task<ForumDto> IApiProxy.GetForumWithSinglePost(int forumId, int postId)
        {
            var forumRequestUri = $"/api/forums/{forumId}/posts/{postId}";

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
        Task<ForumDto> GetForumWithSinglePost(int forumId, int postId);
    }
}