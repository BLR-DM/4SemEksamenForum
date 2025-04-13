namespace ContentService.Application.Commands.CommandDto.PostDto
{
    public record UpdatePostDto(string Title, string Content, uint RowVersion);
}