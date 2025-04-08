using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SubscriptionService.Application.Repositories;
using SubscriptionService.Domain.Entities;

namespace SubscriptionService.Infrastructure.Repositories
{
    public class ForumSubRepository : IForumSubRepository
    {
        private readonly SubscriptionContext _db;

        public ForumSubRepository(SubscriptionContext db)
        {
            _db = db;
        }
        async Task IForumSubRepository.AddAsync(ForumSubscription forumSub)
        {
            _db.ForumSubscriptions.Add(forumSub);
            await _db.SaveChangesAsync();
        }

        async Task IForumSubRepository.DeleteAsync(ForumSubscription forumSub)
        {
            _db.ForumSubscriptions.Remove(forumSub);
            await _db.SaveChangesAsync();
        }

        async Task<ForumSubscription> IForumSubRepository.GetAsync(int forumId, string appUserId)
        {
            return await _db.ForumSubscriptions.FirstOrDefaultAsync(f => f.ForumId == forumId && f.AppUserId == appUserId)
                ?? throw new Exception("Subscription not found.");
        }
    }
}
