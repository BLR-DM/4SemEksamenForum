using SubscriptionService.Application.Queries.QueryDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubscriptionService.Application.Queries.Interfaces
{
    public interface IPostSubQuery
    {
        Task<List<string>> GetSubscriptionsByPostIdAsync(int postId);
        Task<List<int>> GetSubscriptionsByUserIdAsync(string appUserId);
    }
}
