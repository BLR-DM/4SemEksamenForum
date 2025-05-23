﻿namespace WebService.Views
{

    public record ForumView
    {
        public int Id { get; set; }
        public string ForumName { get; set; }
        public string Content { get; set; }
        public string CreatedDate { get; set; }
        public string UserId { get; set; }
        public List<PostView>? Posts { get; set; } = [];
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
        public List<VoteView>? Votes { get; set; }
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
    //    public List<VoteView>? Votes { get; set; }
    //    public List<CommentView>? Comments { get; set; }
    //}

    public record CommentView()
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Content { get; set; }
        public string CreatedDate { get; set; }
        public string UserId { get; set; }
        public int UpVotesCount { get; set; }
        public int DownVotesCount { get; set; }
        public List<VoteView>? Votes { get; set; }
    }

    public record VoteView
    {
        public string UserId { get; set; }
        public bool VoteType { get; set; }
    }

    public record NotificationView
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
        public int SourceId { get; set; }
        public string SourceType { get; set; }
        public int ContextId { get; set; }
        public string ContextType { get; set; }
    }

    public record ForumViewWithPostIds
    {
        public int Id { get; set; }
        public string ForumName { get; set; }
        public string Content { get; set; }
        public string CreatedDate { get; set; }
        public string UserId { get; set; }
        public List<PostWithOnlyId>? Posts { get; set; } = [];
    }
    public record PostWithOnlyId
    {
        public int Id { get; set; }
    }
}
