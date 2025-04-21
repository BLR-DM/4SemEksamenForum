namespace ContentService.Application.Commands.CommandDto.CommentDto
{
    public record RejectCommentDto(int ForumId, int PostId, int CommentId);
}