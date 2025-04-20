namespace ContentService.Application.EventDto.CommentEventDto
{
    public record CommentPublishedDto(string UserId, int ForumId, int PostId, int CommentId);
}