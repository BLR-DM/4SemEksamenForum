using VoteService.Application.Commands.CommandDto;

namespace VoteService.Application.Interfaces;

public interface IPostVoteCommand
{
    Task TogglePostVote(string postId, PostVoteDto dto);
}