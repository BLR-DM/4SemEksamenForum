using VoteService.Application.Commands.CommandDto;

namespace VoteService.Application.Interfaces;

public interface ICommentVoteCommand
{
    Task ToggleCommentVote(int commentId, CommentVoteDto dto, string userId);
}