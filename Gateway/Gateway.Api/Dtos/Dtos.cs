namespace Gateway.Api.Dtos
{
    public record ForumDto
    {
        public int Id { get; set; }
        public string ForumName { get; set; }
        public string Content { get; set; }
        public string CreatedDate { get; set; }
        public string AppUserId { get; set; }
        public List<PostDto> Posts { get; set; }
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
        public List<PostVoteDto>? Votes { get; set; }
    }

    public record CommentDto()
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Content { get; set; }
        public string CreatedDate { get; set; }
        public string AppUserId { get; set; }
    }


    public record PostVoteListDto
    {
        public int PostId { get; set; }
        public List<PostVoteDto> PostVotes { get; set; }
    }

    public record PostVoteDto
    {
        public string UserId { get; set; }
        public bool VoteType { get; set; }
    }
}