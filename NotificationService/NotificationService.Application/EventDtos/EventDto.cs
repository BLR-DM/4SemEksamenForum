namespace NotificationService.Application.EventDtos
{
    public record EventDto(string UserId, int? ForumId, int? PostId, int? CommentId);

    public record PostPublishedEventDto(int ForumId, int PostId);

    public record PostPublishedEventDtoTest(string UserId, int ForumId, int PostId);

    public record ForumSubscribersRequestedEventDto(int ForumId, int PostId);

    public record RequestedForumSubscribersCollectedEventDto(IEnumerable<string> UserIds, int ForumId, int PostId);
}