using VoteService.Application.Commands.CommandDto;

namespace VoteService.Application.Interfaces;

public interface ICommentVoteCommand
{
    Task ToggleCommentVote(string commentId, CommentVoteDto dto);
}