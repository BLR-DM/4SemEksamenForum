namespace NotificationService.Application.EventDtos
{
    public record EventDto(string UserId, int? ForumId, int? PostId, int? CommentId);

    public record PostPublishedDto(string UserId, int ForumId, int PostId, string ForumName);

    public record ForumSubscribersRequestedEventDto(int NotificationId, int ForumId);

    public record RequestedForumSubscribersCollectedEventDto(IEnumerable<string> UserIds, int NotificationId);
}