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
    public class PostSubRepository : IPostSubRepository
    {
        private readonly SubscriptionContext _db;

        public PostSubRepository(SubscriptionContext db)
        {
            _db = db;
        }
        async Task IPostSubRepository.AddAsync(PostSubscription postSub)
        {
            _db.PostSubscriptions.Add(postSub);
            await _db.SaveChangesAsync();
        }

        async Task IPostSubRepository.DeleteAsync(PostSubscription postSub)
        {
            _db.PostSubscriptions.Remove(postSub);
            await _db.SaveChangesAsync();
        }

        async Task<PostSubscription> IPostSubRepository.GetAsync(int postId, string appUserId)
        {
            return await _db.PostSubscriptions.FirstOrDefaultAsync(p => p.PostId == postId && p.AppUserId == appUserId)
                   ?? throw new Exception("Subscription not found."); ;
        }
    }
}
