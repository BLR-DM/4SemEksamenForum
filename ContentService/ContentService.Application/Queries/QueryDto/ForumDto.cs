namespace ContentService.Application.Queries.QueryDto
{
    public record ForumDto
    {
        public int Id { get; set; }
        public string ForumName { get; set; }
        //public string Content { get; set; }
        public string CreatedDate { get; set; }
        public string AppUserId { get; set; }
        public uint RowVersion { get; set; }
        public IEnumerable<PostDto> Posts { get; set; }
    }
}