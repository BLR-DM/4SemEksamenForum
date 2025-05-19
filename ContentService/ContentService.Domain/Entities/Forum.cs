using ContentService.Domain.Enums;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

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
            AppUserId = appUserId;
        }

        public string ForumName { get; protected set; }
        public string Content { get; protected set; }
        public Status Status { get; protected set; } = Status.Submitted;
        public DateTimeOffset CreatedDate { get; protected set; } = DateTimeOffset.UtcNow.AddHours(2);
        public string AppUserId { get; protected set; }
        public IReadOnlyCollection<Post> Posts => _posts;


        // Forum
        public static Forum Create(string forumName, string content, string appUserId, IEnumerable<Forum> otherForums)
        {
            var forum = new Forum(forumName, content, appUserId);

            if (forum.IsForumNameWithSpaces(forumName))
                throw new InvalidOperationException("Forum name cannot have spaces");

            if (!forum.IsForumNameUnique(otherForums))
                throw new InvalidOperationException("Forum name already exists");

            return forum;
        }

        private bool IsForumNameUnique(IEnumerable<Forum> otherForums)
        {
            return !otherForums.Any(other => ForumName.ToLower().Equals(other.ForumName.ToLower()));
        }

        private bool IsForumNameWithSpaces(string forumName)
        {
            return forumName.Any(c => c == ' ');
        }

        public void MarkAsApproved() => Status = Status.Approved;
        public void MarkAsPublished() => Status = Status.Published;
        public void MarkAsRejected() => Status = Status.Rejected;

        public void Update(string content, string appUserId)
        {
            AssureUserIsCreator(appUserId);
            Content = content;
        }

        public IReadOnlyCollection<Post> DeleteAllPosts(string appUserId)
        {
            AssureUserIsCreator(appUserId);

            var deletedPosts = _posts.ToList();
            _posts.Clear();

            return deletedPosts;
        }


        private void AssureUserIsCreator(string userId)
        {
            if (!AppUserId.Equals(userId))
                throw new ArgumentException("Only the creater of the forum can perform this action");
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

        public Post? DeletePost(int postId, string appUserId, out IReadOnlyCollection<Comment> deletedComments)
        {
            var post = Posts.FirstOrDefault(p => p.Id == postId);
            if (post == null)
            {
                deletedComments = new List<Comment>();
                return null;
            }
            
            deletedComments = post.DeleteAllComments();
            post.Delete(appUserId);
            _posts.Remove(post);
            return post;
        }

        public Post GetPostById(int postId)
        {
            var post = Posts.FirstOrDefault(p => p.Id == postId);
            if (post is null) throw new ArgumentException("Post not found");
            return post;
        }
    }
}
