namespace NotificationService.Application.EventDtos
{
    public record EventDto(string UserId, int? ForumId, int? PostId, int? CommentId);

    public record PostPublishedDto(string UserId, int ForumId, int PostId, string ForumName);


    public record ForumSubscribersRequestedEventDto(int NotificationId, int ForumId);
    public record PostSubscribersRequestedEventDto(int NotificationId, int PostId);

    public record RequestedForumSubscribersCollectedEventDto(IEnumerable<string> UserIds, int NotificationId);
    public record RequestedPostSubscribersCollectedEventDto(IEnumerable<string> UserIds, int NotificationId);

    public record CommentPublishedDto(string UserId, int ForumId, int PostId, int CommentId, string Title);
    public record PostVoteEventDto(int PostId, string UserId);
    public record PostRejectedDto(string UserId, int ForumId, int PostId);
    public record ForumRejectedDto(string UserId, int ForumId);
    public record CommentRejectedDto(string UserId, int ForumId, int PostId, int CommentId);

}