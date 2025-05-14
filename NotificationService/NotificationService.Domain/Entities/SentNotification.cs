namespace NotificationService.Domain.Entities
{
    public class SentNotification
    {
        public int NotificationId { get; protected set; }
        public string UserId { get; protected set; }
        public bool IsRead { get; protected set; } = false;

        protected SentNotification() { }

        private SentNotification(int notificationId, string userId)
        {
            NotificationId = notificationId;
            UserId = userId;
        }

        public static SentNotification Create(int notificationId, string userId)
        {
            return new SentNotification(notificationId, userId);
        }

        public void MarkAsRead()
        {
            // Check userId as creator
            IsRead = true;
        }
    }
}