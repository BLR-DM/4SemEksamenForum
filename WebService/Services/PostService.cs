using WebService.Dtos;
using WebService.Dtos.CommandDtos;
using WebService.Helpers;
using WebService.Proxies;
using WebService.Views;

namespace WebService.Services
{
    public class PostService : IPostService
    {
        private readonly IContentServiceProxy _proxy;
        private readonly IApiProxy _apiProxy;

        public PostService(IContentServiceProxy proxy, IApiProxy apiProxy)
        {
            _proxy = proxy;
            _apiProxy = apiProxy;
        }

        async Task IPostService.CreatePost(CreatePostDto dto, int forumId)
        {
            await _proxy.CreatePost(dto, forumId);
        }

        async Task IPostService.CreateComment(CreateCommentDto dto, int forumId, int postId)
        {
            await _proxy.CreateComment(dto, forumId, postId);
        }

        async Task<ForumView> IPostService.GetForumWithSinglePost(string forumName, int postId)
        {
            try
            {
                var forum = await _apiProxy.GetForumWithSinglePost(forumName, postId);

                var forumView = MapDtoToView.MapForumWithPostsToView(forum);

                return forumView;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new ForumView();
            }
        }
    }

    public interface IPostService
    {
        Task CreatePost(CreatePostDto dto, int forumId);
        Task CreateComment(CreateCommentDto dto, int forumId, int postId);
        Task<ForumView> GetForumWithSinglePost(string forumName, int postId);
    }
}