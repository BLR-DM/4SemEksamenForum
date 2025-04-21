namespace VoteService.Application.Services.EventDto
{
    public record PostVoteEventDto(string PostId, string UserId, bool VoteType);
}