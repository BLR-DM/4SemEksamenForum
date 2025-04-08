using System.Runtime.CompilerServices;
using System.Xml.Linq;

namespace ContentService.Domain.Entities
{
    public class Post : DomainEntity
    {
        private readonly List<Comment> _comments = [];
        //private readonly List<PostHistory> _history = [];

        protected Post()
        {
        }

        private Post(string title, string content, string username, string appUserId)
        {
            Title = title;
            Content = content;
            Username = username;
            AppUserId = appUserId;
            CreatedDate = DateTime.Now;
        }

        public string Title { get; protected set; }
        public string Content { get; protected set; }
        public string Username { get; protected set; }
        public string AppUserId { get; protected set; }
        public DateTime CreatedDate { get; private set; }
        //public ICollection<PostHistory> History => _history;
        public IReadOnlyCollection<Comment> Comments => _comments;

        public static Post Create(string title,string content, string username, string appUserId)
        {
            return new Post(title, content, username, appUserId);
        }

        public void Update(string newDescription, string userId)
        {
            AssureUserIsCreator(userId);

            //SetHistory(Description, Solution);
            Content = newDescription;
        }

        //private void SetHistory(string orgDescription, string orgSolution)
        //{
        //    _history.Add(new PostHistory(orgDescription, orgSolution));
        //}

        private void AssureUserIsCreator(string userId)
        {
            if (!AppUserId.Equals(userId))
                throw new ArgumentException("Only the creater of the post can edit this");
        }


        // Comment

        public void CreateComment(string username, string content, string appUserId)
        {
            var comment = Comment.Create(username, content, appUserId);
            _comments.Add(comment);
        }

        public Comment UpdateComment(int commentId, string content, string appUserId)
        {
            var comment = GetCommentById(commentId);

            comment.Update(content, appUserId);
            return comment;
        }

        public Comment DeleteComment(int commentId, string appUserId)
        {
            var comment = GetCommentById(commentId);

            _comments.Remove(comment);
            return comment;
        }

        private Comment GetCommentById(int commentId)
        {
            var comment = Comments.FirstOrDefault(p => p.Id == commentId);
            if (comment is null) throw new ArgumentException("Comment not found");
            return comment;
        }
    }
}
