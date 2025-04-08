namespace VoteService.Application.Commands.CommandDto;

public record PostVoteDto(string UserId, bool VoteType);