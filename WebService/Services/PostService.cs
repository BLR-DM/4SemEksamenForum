using WebService.Dtos.CommandDtos;
using WebService.Proxies;

namespace WebService.Services
{
    public class PostService : IPostService
    {
        private readonly IContentServiceProxy _proxy;

        public PostService(IContentServiceProxy proxy)
        {
            _proxy = proxy;
        }
        async Task IPostService.CreatePost(CreatePostDto dto, int forumId)
        {
            await _proxy.CreatePost(dto, forumId);
        }
    }

    public interface IPostService
    {
        Task CreatePost(CreatePostDto dto, int forumId);
    }
}