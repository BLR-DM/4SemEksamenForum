using VoteService.Application.Commands.CommandDto;
using VoteService.Application.Interfaces;
using VoteService.Application.Services;
using VoteService.Domain.Entities;
using VoteService.Domain.Enums;
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
    private readonly IEventHandler _eventHandler;

    public CommentVoteCommand(CommentVoteService commentVoteService, IEventHandler eventHandler)
    {
        _commentVoteService = commentVoteService;
        _eventHandler = eventHandler;
    }
    async Task ICommentVoteCommand.ToggleCommentVote(string commentId, CommentVoteDto dto)
    {
        var voteAction = await _commentVoteService.ToggleCommentVoteAsync(dto.UserId, commentId, dto.VoteType);

        switch (voteAction)
        {
            case VoteAction.Created:
                await _eventHandler.CommentVoteCreated(commentId, dto.UserId, dto.VoteType);
                break;
            case VoteAction.Deleted:
                await _eventHandler.CommentVoteDeleted(commentId, dto.UserId, dto.VoteType);
                break;
            case VoteAction.Updated:
                await _eventHandler.CommentVoteUpdated(commentId, dto.UserId, dto.VoteType);
                break;
        }
    }
}