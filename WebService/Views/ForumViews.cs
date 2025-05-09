namespace WebService.Views
{

    public record ForumView
    {
        public int Id { get; set; }
        public string ForumName { get; set; }
        public string Content { get; set; }
        public string CreatedDate { get; set; }
        public string UserId { get; set; }
        public List<PostView> Posts { get; set; }
    }

    public record PostView
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
        public List<CommentView>? Comments { get; set; }
    }

    //public record ForumView
    //{
    //    public int Id { get; set; }
    //    public string ForumName { get; set; }
    //    public string Content { get; set; }
    //    public string CreatedDate { get; set; }
    //    public string UserId { get; set; }

    //}

    //public record PostView
    //{
    //    public int Id { get; set; }
    //    public string Title { get; set; }
    //    public string Content { get; set; }
    //    public string Username { get; set; }
    //    public string UserId { get; set; }
    //    public string CreatedDate { get; set; }
    //    public int UpVotesCount { get; set; }
    //    public int DownVotesCount { get; set; }
    //    public int CommentsCount { get; set; }
    //    public List<PostVoteView>? Votes { get; set; }
    //    public List<CommentView>? Comments { get; set; }
    //}

    public record CommentView()
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Content { get; set; }
        public string CreatedDate { get; set; }
        public string UserId { get; set; }
        //public uint RowVersion { get; set; }
    }

    public record PostVoteView
    {
        public string UserId { get; set; }
        public bool VoteType { get; set; }
    }
}
