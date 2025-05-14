namespace WebService.Dtos
{
    public record ForumDto
    {
        public int Id { get; set; }
        public string ForumName { get; set; }
        public string Content { get; set; }
        public string CreatedDate { get; set; }
        public string AppUserId { get; set; }
        public List<PostDto>? Posts { get; set; }
    }

    public record PostDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Username { get; set; }
        public string AppUserId { get; set; }
        public string CreatedDate { get; set; }
        public List<CommentDto>? Comments { get; set; }
        public List<VoteDto>? Votes { get; set; }
    }

    public record CommentDto()
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Content { get; set; }
        public string CreatedDate { get; set; }
        public string AppUserId { get; set; }
        public List<VoteDto>? Votes { get; set; }
    }

    public record VoteDto
    {
        public string UserId { get; set; }
        public bool VoteType { get; set; }
    }

    public record UserPointsDto
    {
        public int Points { get; set; }
    }

    public record NotificationDto
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
