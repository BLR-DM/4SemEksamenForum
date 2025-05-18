namespace PointService.Application.EventDto
{
    public record PostVoteEventDto(int PostId, string UserId);
    public record CommentVoteEventDto(int CommentId, string UserId);
}
