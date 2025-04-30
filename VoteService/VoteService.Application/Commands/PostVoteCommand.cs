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
    async Task IPostVoteCommand.TogglePostVote(string postId, PostVoteDto dto)
    {
        var voteAction = await _postVoteService.TogglePostVoteAsync(dto.UserId, postId, dto.VoteType);

        switch (voteAction)
        {
            case VoteAction.Created:
                if (dto.VoteType == true)
                {
                    await _eventHandler.PostUpVoteCreated(postId, dto.UserId); 
                }
                else
                {
                    await _eventHandler.PostDownVoteCreated(postId, dto.UserId);
                }
                break;

            case VoteAction.Deleted:
                if (dto.VoteType == true)
                {
                    await _eventHandler.PostUpVoteRemoved(postId, dto.UserId); 
                }
                break;

            case VoteAction.Updated:
                if (dto.VoteType == true)
                {
                    await _eventHandler.PostUpVoteCreated(postId, dto.UserId);
                }
                else
                {
                    await _eventHandler.PostDownVoteCreated(postId, dto.UserId);
                }
                break;
        }
    }
}