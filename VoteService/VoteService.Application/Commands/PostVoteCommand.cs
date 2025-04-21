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
    //private readonly IPostVoteRepository _repository;

    //public PostVoteCommand(IPostVoteRepository repository)
    //{
    //    _repository = repository;
    //}

    //async Task IPostVoteCommand.TogglePostVote(string postId, PostVoteDto dto)
    //{
    //    var existingVote = await _repository.GetVoteByUserIdAsync(dto.UserId, postId);

    //    if (existingVote == null)
    //    {
    //        var newVote = PostVote.Create(dto.UserId, postId, dto.VoteType);
    //        await _repository.AddPostVoteAsync(newVote);
    //    }
    //    else if (existingVote.VoteType == dto.VoteType)
    //    {
    //        await _repository.DeletePostVoteAsync(existingVote);
    //    }
    //    else
    //    {
    //        existingVote.Update(dto.VoteType);
    //        await _repository.UpdateVoteAsync(existingVote);
    //    }
    //}
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
                await _eventHandler.PostVoteCreated(postId, dto.UserId, dto.VoteType);
                break;
            case VoteAction.Deleted:
                await _eventHandler.PostVoteDeleted(postId, dto.UserId, dto.VoteType);
                break;
            case VoteAction.Updated:
                await _eventHandler.PostVoteUpdated(postId, dto.UserId, dto.VoteType);
                break;
        }
    }
}