using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SubscriptionService.Application.Queries.Interfaces;
using SubscriptionService.Application.Queries.QueryDto;
using SubscriptionService.Domain.Entities;
using SubscriptionService.Infrastructure.Helpers;

namespace SubscriptionService.Infrastructure.Queries
{
    public class ForumSubQuery : IForumSubQuery
    {
        private readonly SubscriptionContext _db;

        public ForumSubQuery(SubscriptionContext db)
        {
            _db = db;
        }
        async Task<List<string>> IForumSubQuery.GetSubscriptionsByForumIdAsync(int forumId)
        {
            var appUserIds = await _db.ForumSubscriptions.AsNoTracking().Where(f => f.ForumId == forumId).Select(f => f.AppUserId).ToListAsync();

            return appUserIds;
        }

        async Task<List<int>> IForumSubQuery.GetSubscriptionsByUserIdAsync(string appUserId)
        {
            var forumIds = await _db.ForumSubscriptions.AsNoTracking().Where(f => f.AppUserId == appUserId).Select(f => f.ForumId).ToListAsync();

            return forumIds;
        }

        private List<ForumSubDto> MapToForumSubDto(List<ForumSubscription> forumSubs)
        {
            var forumSubDtos = new List<ForumSubDto>();

            foreach (var sub in forumSubs)
            {
                forumSubDtos.Add(SubMapper.MapToForumSubDto(sub));
            }
            return forumSubDtos;
        }
    }
}
