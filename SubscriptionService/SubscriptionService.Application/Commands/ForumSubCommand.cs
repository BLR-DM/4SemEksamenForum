using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SubscriptionService.Application.Commands.CommandDto;
using SubscriptionService.Application.Commands.Interfaces;
using SubscriptionService.Application.Queries.Interfaces;
using SubscriptionService.Application.Repositories;
using SubscriptionService.Application.Services;
using SubscriptionService.Domain.Entities;

namespace SubscriptionService.Application.Commands
{
    public class ForumSubCommand : IForumSubCommand
    {
        private readonly IForumSubRepository _forumSubRepository;
        private readonly IEventHandler _eventHandler;
        private readonly IForumSubQuery _forumSubQuery;

        public ForumSubCommand(IForumSubRepository forumSubRepository, IEventHandler eventHandler, IForumSubQuery forumSubQuery)
        {
            _forumSubRepository = forumSubRepository;
            _eventHandler = eventHandler;
            _forumSubQuery = forumSubQuery;
        }
        async Task IForumSubCommand.CreateAsync(int forumId, string appUserId)
        {
            try
            {
                var otherForumSubs = await _forumSubRepository.GetSubscriptionsByUserIdAsync(appUserId);

                var forumSub = ForumSubscription.Create(forumId, appUserId, otherForumSubs);

                try
                {
                    await _forumSubRepository.AddAsync(forumSub);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }

                await _eventHandler.UserSubscribedToForum(appUserId, forumSub.Id, forumId);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        async Task IForumSubCommand.DeleteAsync(int forumId, string appUserId)
        {
            try
            {
                var forumSub = await _forumSubRepository.GetAsync(forumId, appUserId);

                try
                {
                    await _forumSubRepository.DeleteAsync(forumSub);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }

                await _eventHandler.UserUnsubscribedFromForum(appUserId, forumSub.Id, forumId);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

        }
    }
}
