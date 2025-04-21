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