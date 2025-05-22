using NotificationService.Application.EventDtos;

namespace NotificationService.Application.Helpers
{
    public static class MessageBuilder
    {
        public static string BuildForPostPublished(PostPublishedDto dto)
        {
            return $"A new post have been created on forum: {dto.ForumName}";
        }

        public static string BuildForCommentPublished(CommentPublishedDto dto)
        {
            return $"A new comment has been created on post: {dto.Title}";
        }

        public static string BuildPostVoteCreated()
        {
            return $"A user has interacted with a post you're following";
        }

        public static string BuildPostRejected()
        {
            return $"A post you've tried to create has been rejected";
        }

        public static string BuildForumRejected()
        {
            return $"A forum you've tried to create has been rejected";
        }

        public static string BuildCommentRejected()
        {
            return $"A comment you've tried to create has been rejected";
        }
    }
}