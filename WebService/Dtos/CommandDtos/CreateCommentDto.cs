namespace WebService.Dtos.CommandDtos
{
    public record CreateCommentDto()
    {
        public string Content { get; set; }
    }
}