namespace ContentService.Application.EventDto
{
    public record PostDeletedDto(string UserId, int ForumId, int PostId);
    public record PostPublishedDto(string UserId, int ForumId, int PostId, string ForumName);
    public record PostRejectedDto(string UserId, int ForumId, int PostId); // Reason??
    public record PostSubmittedDto(string ContentId, string Content);
    public record FailedToSubscribeUserToPostEventDto(string UserId, int ForumId, int PostId);
}