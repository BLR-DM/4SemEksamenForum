using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubscriptionService.Domain.Entities
{
    public class PostSubscription
    {
        protected PostSubscription() { AppUserId = string.Empty; }

        public int PostId { get; protected set; }
        public string AppUserId { get; protected set; }
        public DateTime SubscribedAt { get; protected set; } = DateTime.Now;

        private PostSubscription(int postId, string appUserId)
        {
            PostId = postId;
            AppUserId = appUserId;
        }

        public static PostSubscription Create(int postId, string appUserId)
        {
            return new PostSubscription(postId, appUserId);
        }
    }
}
