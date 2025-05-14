using WebService.Dtos;
using WebService.Views;

namespace WebService.Helpers
{
    public class MapDtoToView
    {

        public static ForumView MapForumWithPostsToView(ForumDto forumDto)
        {
            var forumView = new ForumView()
            {
                Id = forumDto.Id,
                ForumName = forumDto.ForumName,
                Content = forumDto.Content,
                CreatedDate = forumDto.CreatedDate,
                UserId = forumDto.AppUserId,
                Posts = forumDto.Posts.Select(p => MapPostWithCommentsToView(p)).ToList()
            };

            return forumView;
        }

        public static ForumView MapForumToForumView(ForumDto forumDto)
        {
            var forumViewShort = new ForumView()
            {
                Id = forumDto.Id,
                ForumName = forumDto.ForumName,
                Content = forumDto.Content,
                CreatedDate = forumDto.CreatedDate,
                UserId = forumDto.AppUserId,
            };

            return forumViewShort;
        }

        public static PostView MapPostWithCommentsToView(PostDto postDto)
        {
            var postView = new PostView()
            {
                Id = postDto.Id,
                Title = postDto.Title,
                Content = postDto.Content,
                Username = postDto.Username,
                UserId = postDto.AppUserId,
                CreatedDate = postDto.CreatedDate,
                UpVotesCount = postDto.Votes?.Count(v => v.VoteType == true) ?? 0,
                DownVotesCount = postDto.Votes?.Count(v => v.VoteType == false) ?? 0,
                CommentsCount = postDto.Comments?.Count ?? 0,
                Votes = postDto.Votes?.Select(v => MapVoteToView(v)).ToList(),
                Comments = postDto.Comments?.Select(c => MapCommentToView(c)).ToList()
            };
            return postView;
        }

        private static CommentView MapCommentToView(CommentDto commentDto)
        {
            var commentView = new CommentView()
            {
                Id = commentDto.Id,
                Username = commentDto.Username,
                Content = commentDto.Content,
                CreatedDate = commentDto.CreatedDate,
                UserId = commentDto.AppUserId,
                UpVotesCount = commentDto.Votes?.Count(v => v.VoteType == true) ?? 0,
                DownVotesCount = commentDto.Votes?.Count(v => v.VoteType == false) ?? 0,
                Votes = commentDto.Votes?.Select(v => MapVoteToView(v)).ToList(),
            };
            return commentView;
        }

        public static VoteView MapVoteToView(VoteDto voteDto)
        {
            var voteView = new VoteView()
            {
                UserId = voteDto.UserId,
                VoteType = voteDto.VoteType
            };

            return voteView;
        }

        public static NotificationView MapNotificationToView(NotificationDto notificationDto)
        {
            var notificationView = new NotificationView()
            {
                Id = notificationDto.Id,
                UserId = notificationDto.UserId,
                Message = notificationDto.Message,
                NotificationRead = notificationDto.NotificationRead,
                CreatedAt = notificationDto.CreatedAt
            };

            return notificationView;
        }
    }
}
