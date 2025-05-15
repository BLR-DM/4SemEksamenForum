namespace ContentService.Application.EventDto
{
    public record ForumDeletedDto(string UserId, int ForumId);
    public record ForumPublishedDto(string UserId, int ForumId);
    public record ForumRejectedDto(string UserId, int ForumId); // Reason??
    public record ForumSubmittedDto(string ContentId, string Content);
    public record CompensateByDeletingForumDto(string UserId, int ForumId);
    public record CompensateByDeletingPostDto(string UserId, int ForumId, int PostId);
}
