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

        public PostService(IContentServiceProxy proxy)
        {
            _proxy = proxy;
        }

        async Task IPostService.CreatePost(CreatePostDto dto, int forumId)
        {
            await _proxy.CreatePost(dto, forumId);
        }

        async Task IPostService.CreateComment(CreateCommentDto dto, int forumId, int postId)
        {
            await _proxy.CreateComment(dto, forumId, postId);
        }

        async Task<PostView> IPostService.GetPostWithComments(int forumId, int postId)
        {
            try
            {
                var forum = await _proxy.GetForumWithSinglePostAsync(forumId, postId);

                var postView = MapDtoToView.MapPostWithCommentsToView(forum.Posts.First());

                return postView;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new PostView();
            }
        }
    }

    public interface IPostService
    {
        Task CreatePost(CreatePostDto dto, int forumId);
        Task CreateComment(CreateCommentDto dto, int forumId, int postId);
        Task<PostView> GetPostWithComments(int forumId, int postId);
    }
}