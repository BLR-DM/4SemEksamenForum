namespace PointService.Application.EventDto
{
    public record PostPublishedDto(string UserId, int ForumId, int PostId);
    public record PostDeletedDto(string UserId, int ForumId, int PostId);

    public record FailedToAddPointsOnPostPublishedDto(string UserId, int ForumId, int PostId);

}
