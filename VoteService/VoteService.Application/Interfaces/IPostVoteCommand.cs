using VoteService.Application.Commands.CommandDto;

namespace VoteService.Application.Interfaces;

public interface IPostVoteCommand
{
    Task TogglePostVote(int postId, PostVoteDto dto, string userId);
}