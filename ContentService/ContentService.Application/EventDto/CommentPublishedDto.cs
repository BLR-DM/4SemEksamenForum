namespace ContentService.Application.EventDto
{
    public record CommentPublishedDto(string UserId, int ForumId, int PostId, int CommentId);
}