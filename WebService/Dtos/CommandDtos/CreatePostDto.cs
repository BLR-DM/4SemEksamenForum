namespace WebService.Dtos.CommandDtos
{
    public record CreatePostDto()
    {
        public string Title { get; set; }
        public string Content { get; set; }
    }
}