using WebService.Dtos;
using WebService.Helpers;
using WebService.Proxies;
using WebService.Views;

namespace WebService.Services
{
    public class ForumService : IForumService
    {
        private readonly IApiProxy _apiProxy;

        public ForumService(IApiProxy apiProxy)
        {
            _apiProxy = apiProxy;
        }
        async Task<ForumViewShort> IForumService.GetForumWithPostsShort(string forumName)
        {
            var forum = await _apiProxy.GetForumWithPosts(forumName);

            var forumViewShort = MapDtoToView.MapForumWithPostsShortToView(forum);

            return forumViewShort;
        }
    }

    public interface IForumService
    {

        Task<ForumViewShort> GetForumWithPostsShort(string forumName);
    }
}