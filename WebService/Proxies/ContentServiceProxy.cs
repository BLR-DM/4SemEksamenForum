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

        async Task IContentServiceProxy.CreatePost(CreatePostDto dto, int forumId)
        {
            try
            {
                var uri = $"api/content/forum/{forumId}/post";

                var response = await _httpClient.PostAsJsonAsync(uri, dto);

                if (!response.IsSuccessStatusCode)
                    throw new Exception("Failed to create post");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new Exception("Failed to create post");
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

        async Task<ForumDto> IContentServiceProxy.GetForumWithSinglePostAsync(int forumId, int postId)
        {
            try
            {
                var forumReuqestUri = $"/api/content/forum/{forumId}/post/{postId}";

                var forum = await _httpClient.GetFromJsonAsync<ForumDto>(forumReuqestUri);

                if (forum == null)
                {
                    throw new Exception("No forum Found");
                }

                return forum;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new ForumDto();
            }
        }
    }

    public interface IContentServiceProxy
    {
        Task<List<ForumDto>> GetForumsAsync();
        Task<ForumDto> GetForumWithSinglePostAsync(int forumId, int postId);
        Task CreateForum(CreateForumDto dto);
        Task CreatePost(CreatePostDto dto, int forumId);
    }
}
