namespace ContentService.Application.EventDto
{
    public record CommentDeletedDto(string UserId, int ForumId, int PostId, int CommentId);
    public record CommentPublishedDto(string UserId, int ForumId, int PostId, int CommentId);
    public record CommentRejectedDto(string UserId, int ForumId, int PostId, int CommentId);
    public record CommentSubmittedDto(string ContentId, string Content);
}