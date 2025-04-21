namespace ContentService.Application.EventDto.CommentEventDto
{
    public record CommentRejectedDto(string UserId, int ForumId, int PostId, int CommentId);
}