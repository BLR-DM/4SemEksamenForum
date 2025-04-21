namespace VoteService.Application.Services.EventDto
{
    public record CommentVoteEventDto(string CommentId, string UserId, bool VoteType);
}