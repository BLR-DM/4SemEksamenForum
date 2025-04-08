namespace ContentService.Application.Commands.CommandDto.CommentDto
{
    public record UpdateCommentDto(string Content, uint RowVersion);
}