namespace ContentService.Application.EventDto.CommentEventDto
{
    public record CommentDeletedDto(int ForumId, int PostId, int CommentId);
}