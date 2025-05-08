namespace WebService.Views
{

    public record ForumViewShort
    {
        public int Id { get; set; }
        public string ForumName { get; set; }
        public string Content { get; set; }
        public string CreatedDate { get; set; }
        public string UserId { get; set; }
        public List<PostViewShort> Posts { get; set; }

    }

    public record PostViewShort
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Username { get; set; }
        public string UserId { get; set; }
        public string CreatedDate { get; set; }

        public int UpVotesCount { get; set; }
        public int DownVotesCount { get; set; }
        public int CommentsCount { get; set; }
        public List<PostVoteView>? Votes { get; set; }

    }

    public record ForumView
    {
        public int Id { get; set; }
        public string ForumName { get; set; }
        public string Content { get; set; }
        public string CreatedDate { get; set; }
        public string UserId { get; set; }

    }

    public record PostVoteView
    {
        public string UserId { get; set; }
        public bool VoteType { get; set; }
    }
}
