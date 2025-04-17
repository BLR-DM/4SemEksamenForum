using ContentService.Domain.Enums;

namespace ContentService.Domain.Entities
{
    public class Comment : DomainEntity
    {
        protected Comment()
        {
        }

        private Comment(string username, string content, string appUserId)
        {
            Username = username;
            Content = content;
            AppUserId = appUserId;
        }
        
        public string Content { get; protected set; }
        public string Username { get; protected set; }
        public Status Status { get; protected set; } = Status.Submitted;
        public DateTimeOffset CreatedDate { get; protected set; } = DateTimeOffset.UtcNow.AddHours(2);
        public string AppUserId { get; protected set; }

        public static Comment Create(string username, string content, string appUserId)
        {
            return new Comment(username, content, appUserId);
        }

        public void MarkAsApproved() => Status = Status.Approved;
        public void MarkAsPublished() => Status = Status.Published;
        public void MarkAsRejected() => Status = Status.Rejected;

        public void Update(string content, string appUserId)
        {
            AssureUserIsCreator(appUserId);
            Content = content;
        }

        private void AssureUserIsCreator(string userId)
        {
            if (!AppUserId.Equals(userId))
                throw new ArgumentException("Only the creater of the comment can edit this");
        }
    }
}
