namespace NotificationService.Application.Helpers
{
    public static class EventNames
    {
        public const string PostPublished = "post-published";
        public const string RequestedForumSubscribersCollected = "requested-forum-subscribers-collected";
        public const string CommentPublished = "comment-published";
        public const string RequestedPostSubscribersCollected = "requested-post-subscribers-collected";
        public const string PostUpVoteCreated = "post-upvote-created";
        public const string PostDownVoteCreated = "post-downvote-created";
        public const string PostVoteCreated = "post-vote-created";
        public const string PostRejected = "post-rejected";
        public const string ForumRejected = "forum-rejected";
        public const string CommentRejected = "comment-rejected";
    }
}