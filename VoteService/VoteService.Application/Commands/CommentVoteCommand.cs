using VoteService.Application.Commands.CommandDto;
using VoteService.Application.Interfaces;
using VoteService.Domain.Entities;
using VoteService.Domain.Interfaces;
using VoteService.Domain.Services;

namespace VoteService.Application.Commands;

public class CommentVoteCommand : ICommentVoteCommand
{
    //private readonly ICommentVoteRepository _repository;

    //public CommentVoteCommand(ICommentVoteRepository repository)
    //{
    //    _repository = repository;
    //}
    //async Task ICommentVoteCommand.ToggleCommentVote(string commentId, CommentVoteDto dto)
    //{
    //    var existingVote = await _repository.GetVoteByUserIdAsync(dto.UserId, commentId);

    //    if (existingVote == null)
    //    {
    //        var newVote = CommentVote.Create(dto.UserId, commentId, dto.VoteType);
    //        await _repository.AddCommentVoteAsync(newVote);
    //    }
    //    else if (existingVote.VoteType == dto.VoteType)
    //    {
    //        await _repository.DeleteCommentVoteAsync(existingVote);
    //    }
    //    else
    //    {
    //        existingVote.Update(dto.VoteType);
    //        await _repository.UpdateVoteAsync(existingVote);
    //    }
    //}
    private readonly CommentVoteService _commentVoteService;
    public CommentVoteCommand(CommentVoteService commentVoteService)
    {
        _commentVoteService = commentVoteService;
    }
    async Task ICommentVoteCommand.ToggleCommentVote(string commentId, CommentVoteDto dto)
    {
        await _commentVoteService.ToggleCommentVoteAsync(dto.UserId, commentId, dto.VoteType);
    }
}