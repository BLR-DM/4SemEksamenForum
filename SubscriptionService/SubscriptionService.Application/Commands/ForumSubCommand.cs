using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SubscriptionService.Application.Commands.CommandDto;
using SubscriptionService.Application.Commands.Interfaces;
using SubscriptionService.Application.Repositories;
using SubscriptionService.Application.Services;
using SubscriptionService.Domain.Entities;

namespace SubscriptionService.Application.Commands
{
    public class ForumSubCommand : IForumSubCommand
    {
        private readonly IForumSubRepository _forumSubRepository;
        private readonly IEventHandler _eventHandler;

        public ForumSubCommand(IForumSubRepository forumSubRepository, IEventHandler eventHandler)
        {
            _forumSubRepository = forumSubRepository;
            _eventHandler = eventHandler;
        }
        async Task IForumSubCommand.CreateAsync(int forumId, string appUserId)
        {
            var forumSub = ForumSubscription.Create(forumId, appUserId);

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

        async Task IForumSubCommand.DeleteAsync(int forumId, string appUserId)
        {
            var forumSub = await _forumSubRepository.GetAsync(forumId, appUserId);
           
            await _forumSubRepository.DeleteAsync(forumSub);

            await _eventHandler.UserUnsubscribedFromForum(appUserId, forumSub.Id, forumId);

        }
    }
}
