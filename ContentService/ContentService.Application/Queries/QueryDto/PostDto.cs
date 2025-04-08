namespace ContentService.Application.Queries.QueryDto
{
    public record PostDto
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Username { get; set; }
        public string AppUserId { get; set; }
        public string CreatedDate { get; set; }
        public uint RowVersion { get; set; }

        //public IEnumerable<PostHistoryDto> History { get; set; }
        //public IEnumerable<VoteDto> Votes { get; set; }
        public IEnumerable<CommentDto> Comments { get; set; }
    }
}