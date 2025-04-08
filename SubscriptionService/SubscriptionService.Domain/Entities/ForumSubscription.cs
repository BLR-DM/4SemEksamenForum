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

        public int ForumId { get; protected set; }
        public string AppUserId { get; protected set; }
        public DateTime SubscribedAt { get; protected set; } = DateTime.UtcNow.AddHours(1);

        private ForumSubscription(int forumId, string appUserId)
        {
            ForumId = forumId;
            AppUserId = appUserId;
            
        }

        public static ForumSubscription Create(int forumId, string appUserId)
        {
            return new ForumSubscription(forumId, appUserId);
        }
    }
}
