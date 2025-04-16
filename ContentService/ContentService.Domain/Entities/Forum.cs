﻿using ContentService.Domain.Enums;

namespace ContentService.Domain.Entities
{
    public class Forum : DomainEntity
    {
        private readonly List<Post> _posts = [];

        protected Forum()
        {
        }

        private Forum(string forumName, string content, string appUserId)
        {
            ForumName = forumName;
            Content = content;
            CreatedDate = DateTime.Now;
            AppUserId = appUserId;
            Status = Status.Submitted;
        }

        public string ForumName { get; protected set; }
        public string Content { get; protected set; }
        public DateTime CreatedDate { get; protected set; }
        public string AppUserId { get; protected set; }
        public Status Status { get; protected set; }
        public IReadOnlyCollection<Post> Posts => _posts;


        // Forum

        public static Forum Create(string forumName, string content, string appUserId)
        {
            return new Forum(forumName, content, appUserId);
        }

        public void Approve()
        {
            if (Status != Status.Submitted)
                throw new InvalidOperationException("Only submitted forums can be approved");

            Status = Status.Approved;
        }

        public void Publish()
        {
            //if (Status != Status.Approved)
            //    throw new InvalidOperationException("Only approved forums can be published");

            Status = Status.Published;
        }

        public void Update(string content, string appUserId)
        {
            AssureUserIsCreator(appUserId);
            Content = content;
        }

        private void AssureUserIsCreator(string userId)
        {
            if (!AppUserId.Equals(userId))
                throw new ArgumentException("Only the creater of the post can edit this");
        }

        // Post

        public Post AddPost(string title, string content, string username, string appUserId)
        {
            var post = Post.Create(title, content, username, appUserId);
            _posts.Add(post);
            return post;
        }

        public Post UpdatePost(int postId, string title, string content, string appUserId)
        {
            var post = GetPostById(postId);
            post.Update(title, content, appUserId);
            return post;
        }

        public Post DeletePost(int postId, string appUserId) //maaske slet? //admin ??
        {
            var post = GetPostById(postId);
            _posts.Remove(post);
            return post;
        }

        public Post GetPostById(int postId)
        {
            var post = Posts.SingleOrDefault(p => p.Id == postId);
            if (post is null) throw new ArgumentException("Post not found");
            return post;
        }
    }
}
