using SubscriptionService.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SubscriptionService.Application.Commands.Interfaces;
using SubscriptionService.Application.Commands.CommandDto;
using SubscriptionService.Application.Services;
using SubscriptionService.Domain.Entities;

namespace SubscriptionService.Application.Commands
{
    public class PostSubCommand : IPostSubCommand
    {
        private readonly IPostSubRepository _postSubRepository;
        private readonly IEventHandler _eventHandler;

        public PostSubCommand(IPostSubRepository postSubRepository, IEventHandler eventHandler)
        {
            _postSubRepository = postSubRepository;
            _eventHandler = eventHandler;
        }
        async Task IPostSubCommand.CreateAsync(int postId, string userId)
        {
            try
            {
                var otherSubscriptions = await _postSubRepository.GetSubscriptionsByUserIdAsync(userId);

                var postSub = PostSubscription.Create(postId, userId, otherSubscriptions);

                try
                {
                    await _postSubRepository.AddAsync(postSub);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }

                await _eventHandler.UserSubscribedToPost(userId, postSub.Id, postId);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

        }

        async Task IPostSubCommand.DeleteAsync(int postId, string userId)
        {
            try
            {
                var postSub = await _postSubRepository.GetAsync(postId, userId);

                try
                {
                    await _postSubRepository.DeleteAsync(postSub);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }

                await _eventHandler.UserUnsubscribedFromPost(userId, postSub.Id, postId);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
