using WebService.Dtos;
using WebService.Dtos.CommandDtos;
using WebService.Helpers;
using WebService.Proxies;
using WebService.Views;

namespace WebService.Services
{
    public class ForumService : IForumService
    {
        private readonly IApiProxy _apiProxy;
        private readonly IContentServiceProxy _contentServiceProxy;

        public ForumService(IApiProxy apiProxy, IContentServiceProxy contentServiceProxy)
        {
            _apiProxy = apiProxy;
            _contentServiceProxy = contentServiceProxy;
        }
        async Task<ForumView?> IForumService.GetForumWithPostsShort(string forumName)
        {
            try
            {
                var forum = await _apiProxy.GetForumWithPosts(forumName);

                var forumViewShort = MapDtoToView.MapForumWithPostsToView(forum);

                return forumViewShort;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        async Task<List<string>> IForumService.GetForumNames()
        {
            var forums = await _contentServiceProxy.GetForumsAsync();

            var forumNamesList = forums.Select(forum => forum.ForumName).ToList();

            return forumNamesList;
        }

        async Task<List<ForumView>> IForumService.GetForums()
        {
            var forums = await _contentServiceProxy.GetForumsAsync();

            if (forums == null)
                return new List<ForumView>();


            var forumViews = forums.Select(f => MapDtoToView.MapForumToForumView(f)).ToList();

            return forumViews;
        }

        async Task IForumService.CreateForum(CreateForumDto dto)
        {
            try
            {
                await _contentServiceProxy.CreateForum(dto);
            }
            catch (Exception)
            {
                throw;
            }
        }

        async Task<List<ForumViewWithPostIds>> IForumService.GetForumsWithPostsIds()
        {
            var forums = await _contentServiceProxy.GetForumsWithPostsAsync();

            if (forums == null)
                return new List<ForumViewWithPostIds>();

            var forumViews = forums.Select(f => MapDtoToView.MapForumWithPostsIdsToView(f)).ToList();

            return forumViews;
        }
    }

    public interface IForumService
    {

        Task<ForumView> GetForumWithPostsShort(string forumName);
        Task<List<string>> GetForumNames();
        Task<List<ForumView>> GetForums();
        Task<List<ForumViewWithPostIds>> GetForumsWithPostsIds();
        Task CreateForum(CreateForumDto dto);
    }
}