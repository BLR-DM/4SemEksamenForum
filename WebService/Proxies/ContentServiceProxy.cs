using System.Net.Http.Json;
using WebService.Dtos;
using WebService.Dtos.CommandDtos;
using WebService.Pages;

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
                var uri = "content/forum";

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
                var uri = $"content/forum/{forumId}/post";

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

        async Task IContentServiceProxy.CreateComment(CreateCommentDto dto, int forumId, int postId)
        {
            try
            {
                var uri = $"content/forum/{forumId}/post/{postId}/comment";

                var response = await _httpClient.PostAsJsonAsync(uri, dto);

                if (!response.IsSuccessStatusCode)
                    throw new Exception("Failed to create comment");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new Exception("Failed to create comment");
            }
        }

        async Task<List<ForumDto>> IContentServiceProxy.GetForumsAsync()
        {
            try
            {
                var forumReuqestUri = "content/forum";

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

        async Task<ForumDto> IContentServiceProxy.GetForumByNameWithSinglePostAsync(string forumName, int postId)
        {
            try
            {
                var forumReuqestUri = $"content/forum/{forumName}/post/{postId}";

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

        async Task IContentServiceProxy.DeletePost(int forumId, int postId)
        {
            try
            {
                var uri = $"content/forum/{forumId}/post/{postId}";

                var response = await _httpClient.DeleteAsync(uri);

                if(!response.IsSuccessStatusCode)
                    throw new Exception("Could not delete post");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new Exception("Could not delete post");
            }
        }

        async Task IContentServiceProxy.DeleteComment(int forumId, int postId, int commentId)
        {
            try
            {
                var uri = $"content/forum/{forumId}/post/{postId}/comment/{commentId}";

                var response = await _httpClient.DeleteAsync(uri);

                if (!response.IsSuccessStatusCode)
                    throw new Exception("Could not delete comment");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new Exception("Could not delete comment");
            }
        }

        async Task<List<ForumDto>> IContentServiceProxy.GetForumsWithPostsAsync()
        {
            try
            {
                var forumReuqestUri = $"content/forums/posts";

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
        Task<List<ForumDto>> GetForumsWithPostsAsync();
        Task<ForumDto> GetForumByNameWithSinglePostAsync(string forumName, int postId);
        Task CreateForum(CreateForumDto dto);
        Task CreatePost(CreatePostDto dto, int forumId);
        Task CreateComment(CreateCommentDto dto, int forumId, int postId);
        Task DeletePost(int forumId, int postId);
        Task DeleteComment(int forumId, int postId, int commentId);
    }
}
