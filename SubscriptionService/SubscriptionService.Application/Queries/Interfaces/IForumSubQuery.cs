using SubscriptionService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SubscriptionService.Application.Dto;
using SubscriptionService.Application.Queries.QueryDto;

namespace SubscriptionService.Application.Queries.Interfaces
{
    public interface IForumSubQuery
    {
        Task<List<string>> GetAppUserIdForSubscriptionsByForumIdAsync(int forumId);
        Task<List<int>> GetForumIdForSubscriptionsByUserIdAsync(string appUserId);
    }
}
