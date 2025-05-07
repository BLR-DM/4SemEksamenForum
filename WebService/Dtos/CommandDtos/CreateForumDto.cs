namespace WebService.Dtos.CommandDtos
{
    public record CreateForumDto()
    {
        public string ForumName { get; set; }
        public string Content { get; set; } 
    }
}