namespace ContentService.Application.Queries.QueryDto
{
    public record CommentDto()
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Content { get; set; }
        public string CreatedDate { get; set; }
        public string AppUserId { get; set; }
        public uint RowVersion { get; set; }
    }
}