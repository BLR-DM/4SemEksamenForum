using VoteService.Application.Commands.CommandDto;
using VoteService.Application.Interfaces;
using VoteService.Application.Services;
using VoteService.Domain.Entities;
using VoteService.Domain.Enums;
using VoteService.Domain.Interfaces;
using VoteService.Domain.Services;

namespace VoteService.Application.Commands;

public class PostVoteCommand : IPostVoteCommand
{
    private readonly PostVoteService _postVoteService;
    private readonly IEventHandler _eventHandler;

    public PostVoteCommand(PostVoteService postVoteService, IEventHandler eventHandler)
    {
        _postVoteService = postVoteService;
        _eventHandler = eventHandler;
    }
    async Task IPostVoteCommand.TogglePostVote(int postId, PostVoteDto dto, string userId)
    {
        var voteAction = await _postVoteService.TogglePostVoteAsync(userId, postId, dto.VoteType);

        switch (voteAction)
        {
            case VoteAction.Created:
                if (dto.VoteType == true)
                {
                    await _eventHandler.PostUpVoteCreated(postId, userId); 
                }
                else
                {
                    await _eventHandler.PostDownVoteCreated(postId, userId);
                }
                break;

            case VoteAction.Deleted:
                if (dto.VoteType == true)
                {
                    await _eventHandler.PostUpVoteRemoved(postId, userId); 
                }
                break;

            case VoteAction.Updated:
                if (dto.VoteType == true)
                {
                    await _eventHandler.PostUpVoteCreated(postId, userId);
                }
                else
                {
                    await _eventHandler.PostDownVoteCreated(postId, userId);
                }
                break;
        }
    }
}