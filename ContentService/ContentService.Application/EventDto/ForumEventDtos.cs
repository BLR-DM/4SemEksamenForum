namespace ContentService.Application.EventDto
{
    public record ForumEventDtos(int ForumId);
    public record ForumPublishedDto(string UserId, int ForumId);
    public record ForumRejectedDto(string UserId, int ForumId); // Reason??
    public record ForumSubmittedDto(string ContentId, string Content);
}
