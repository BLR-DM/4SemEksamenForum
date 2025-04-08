using VoteService.Application.Commands.CommandDto;
using VoteService.Application.Interfaces;
using VoteService.Domain.Entities;
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
    public PostVoteCommand(PostVoteService postVoteService)
    {
        _postVoteService = postVoteService;
    }
    async Task IPostVoteCommand.TogglePostVote(string postId, PostVoteDto dto)
    {
        await _postVoteService.TogglePostVoteAsync(dto.UserId, postId, dto.VoteType);
    }
}