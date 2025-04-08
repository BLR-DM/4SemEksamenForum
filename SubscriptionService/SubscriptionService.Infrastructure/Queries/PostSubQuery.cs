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
    public class PostSubQuery : IPostSubQuery
    {
        private readonly SubscriptionContext _db;

        public PostSubQuery(SubscriptionContext db)
        {
            _db = db;
        }

        async Task<List<string>> IPostSubQuery.GetSubscriptionsByPostIdAsync(int postId)
        {
            var appUserIds = await _db.PostSubscriptions.Where(p => p.PostId == postId).Select(p => p.AppUserId).ToListAsync();

            return appUserIds;
        }

        async Task<List<int>> IPostSubQuery.GetSubscriptionsByUserIdAsync(string appUserId)
        {
            var postIds = await _db.PostSubscriptions.Where(p => p.AppUserId == appUserId).Select(p => p.PostId).ToListAsync();

            return postIds;
        }

        private List<PostSubDto> MapToPostSubDto(List<PostSubscription> postSubs)
        {
            var postSubDtos = new List<PostSubDto>();

            foreach (var sub in postSubs)
            {
                postSubDtos.Add(SubMapper.MapToPostSubDto(sub));
            }

            return postSubDtos;
        }
    }
}
