using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubscriptionService.Domain.Entities
{
    public class ForumSubscription
    {
        protected ForumSubscription() { AppUserId = string.Empty; }

        public int Id { get; protected set; }
        public int ForumId { get; protected set; }
        public string AppUserId { get; protected set; }
        public DateTimeOffset SubscribedAt { get; protected set; } = DateTimeOffset.UtcNow.AddHours(2);

        private ForumSubscription(int forumId, string appUserId, List<ForumSubscription> otherSubscriptions)
        {
            ForumId = forumId;
            AppUserId = appUserId;
            AssureUserDoesntAlreadySubscribe(otherSubscriptions);
        }

        public static ForumSubscription Create(int forumId, string appUserId, List<ForumSubscription> otherSubscriptions)
        {
            return new ForumSubscription(forumId, appUserId, otherSubscriptions);
        }

        private void AssureUserDoesntAlreadySubscribe(List<ForumSubscription> otherSubscriptions)
        {
            if (otherSubscriptions.Any(sub => sub.ForumId == ForumId && sub.AppUserId == AppUserId))
            {
                throw new Exception("User is already subscribed to this forum.");
            }
        }
    }
}
