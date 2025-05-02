using WebService.Dtos;
using WebService.Views;

namespace WebService.Helpers
{
    public class MapDtoToView
    {

        public static ForumViewShort MapForumWithPostsShortToView(ForumDto forumDto)
        {
            var forumViewShort = new ForumViewShort()
            {
                Id = forumDto.Id.ToString(),
                ForumName = forumDto.ForumName,
                Content = forumDto.Content,
                CreatedDate = forumDto.CreatedDate,
                UserId = forumDto.AppUserId,
                Posts = forumDto.Posts.Select(p => MapPostToViewShort(p)).ToList()
            };

            return forumViewShort;
        }

        public static PostViewShort MapPostToViewShort(PostDto postDto)
        {
            var postViewShort = new PostViewShort()
            {
                Id = postDto.Id,
                Title = postDto.Title,
                Content = postDto.Content,
                Username = postDto.Username,
                UserId = postDto.AppUserId,
                CreatedDate = postDto.CreatedDate,
                UpVotesCount = postDto.Votes?.Count(v => v.VoteType == true) ?? 0,
                DownVotesCount = postDto.Votes?.Count(v => v.VoteType == false) ?? 0,
                CommentsCount = postDto.Comments?.Count ?? 0
            };
            return postViewShort;
        }
    }
}
