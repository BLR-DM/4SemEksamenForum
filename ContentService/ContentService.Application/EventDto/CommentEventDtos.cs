namespace ContentService.Application.EventDto
{
    public record CommentEventDtos(int ForumId, int PostId, int CommentId);
    public record CommentPublishedDto(string UserId, int ForumId, int PostId, int CommentId);
    public record CommentRejectedDto(string UserId, int ForumId, int PostId, int CommentId);
    public record CommentSubmittedDto(string ContentId, string Content);
}