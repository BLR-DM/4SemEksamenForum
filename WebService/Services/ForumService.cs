using WebService.Dtos;
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
        async Task<ForumViewShort?> IForumService.GetForumWithPostsShort(string forumName)
        {
            try
            {
                var forum = await _apiProxy.GetForumWithPosts(forumName);

                var forumViewShort = MapDtoToView.MapForumWithPostsShortToView(forum);

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

    }

    public interface IForumService
    {

        Task<ForumViewShort> GetForumWithPostsShort(string forumName);

        Task<List<string>> GetForumNames();
    }
}