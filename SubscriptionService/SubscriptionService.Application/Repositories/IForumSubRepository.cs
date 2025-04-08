using SubscriptionService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubscriptionService.Application.Repositories
{
    public interface IForumSubRepository
    {
        Task AddAsync(ForumSubscription forumSub);
        Task DeleteAsync(ForumSubscription forumSub);
        Task<ForumSubscription> GetAsync(int forumId, string appUserId);
    }
}
