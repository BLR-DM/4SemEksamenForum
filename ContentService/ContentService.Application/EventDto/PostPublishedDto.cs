namespace ContentService.Application.EventDto
{
    public record PostPublishedDto(string UserId, int ForumId, int PostId);
}