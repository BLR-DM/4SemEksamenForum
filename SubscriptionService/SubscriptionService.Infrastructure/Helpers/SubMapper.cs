using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SubscriptionService.Application.Queries.QueryDto;
using SubscriptionService.Domain.Entities;
using SubscriptionService.Infrastructure.Queries;

namespace SubscriptionService.Infrastructure.Helpers
{
    public static class SubMapper
    {
        public static ForumSubDto MapToForumSubDto(ForumSubscription forumSub)
        {
            var forumSubDto = new ForumSubDto
            {
                AppUserId = forumSub.AppUserId,
                ItemId = forumSub.ForumId,
                SubscribedAt = forumSub.SubscribedAt
            };

            return forumSubDto;
        }

        public static PostSubDto MapToPostSubDto(PostSubscription postSub)
        {
            var postSubDto = new PostSubDto
            {
                AppUserId = postSub.AppUserId,
                PostId = postSub.PostId,
                SubscribedAt = postSub.SubscribedAt
            };

            return postSubDto;
        }
    }
}
