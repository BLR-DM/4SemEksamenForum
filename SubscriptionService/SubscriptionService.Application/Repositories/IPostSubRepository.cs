using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SubscriptionService.Domain.Entities;

namespace SubscriptionService.Application.Repositories
{
    public interface IPostSubRepository
    {
        Task AddAsync(PostSubscription postSub);
        Task DeleteAsync(PostSubscription postSub); 
        Task<PostSubscription> GetAsync(int postId, string appUserId);

    }
}
