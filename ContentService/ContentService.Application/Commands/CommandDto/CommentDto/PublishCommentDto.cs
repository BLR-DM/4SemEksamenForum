namespace ContentService.Application.Commands.CommandDto.CommentDto
{
    public record PublishCommentDto(int ForumId, int PostId, int CommentId);
}