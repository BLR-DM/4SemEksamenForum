namespace ContentService.Application.Commands.CommandDto.PostDto
{
    public record UpdatePostDto(string Title, string Description, uint RowVersion);
}