using SubscriptionService.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SubscriptionService.Application.Commands.Interfaces;
using SubscriptionService.Application.Commands.CommandDto;
using SubscriptionService.Domain.Entities;

namespace SubscriptionService.Application.Commands
{
    public class PostSubCommand : IPostSubCommand
    {
        private readonly IPostSubRepository _postSubRepository;

        public PostSubCommand(IPostSubRepository postSubRepository)
        {
            _postSubRepository = postSubRepository;
        }
        async Task IPostSubCommand.CreateAsync(int postId, string appUserId)
        {
            var postSub = PostSubscription.Create(postId, appUserId);

            await _postSubRepository.AddAsync(postSub);
        }

        async Task IPostSubCommand.DeleteAsync(int postId, string appUserId)
        {
            var postSub = await _postSubRepository.GetAsync(postId, appUserId);

            await _postSubRepository.DeleteAsync(postSub);
        }
    }
}
