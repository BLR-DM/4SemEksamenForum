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

        public int Id { get; protected set; }
        public int PostId { get; protected set; }
        public string AppUserId { get; protected set; }
        public DateTimeOffset SubscribedAt { get; protected set; } = DateTimeOffset.UtcNow.AddHours(2);

        private PostSubscription(int postId, string appUserId, List<PostSubscription> otherSubscriptions)
        {
            PostId = postId;
            AppUserId = appUserId;
            AssureUserDoesntAlreadySubscribe(otherSubscriptions);
        }

        public static PostSubscription Create(int postId, string appUserId, List<PostSubscription> otherSubscriptions)
        {
            return new PostSubscription(postId, appUserId, otherSubscriptions);
        }

        private void AssureUserDoesntAlreadySubscribe(List<PostSubscription> otherSubscriptions)
        {
            if (otherSubscriptions.Any(sub => sub.PostId == PostId && sub.AppUserId == AppUserId))
            {
                throw new Exception("User is already subscribed to this post.");
            }
        }
    }
}
