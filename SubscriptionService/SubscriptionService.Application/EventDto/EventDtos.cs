namespace SubscriptionService.Application.EventDto
{
    public record UserSubscribedToForumEventDto(string UserId, int SubscriptionId, int ForumId);

    public record FailedToSubscribeUserToForumEventDto(string UserId, int ForumId);

    public record UserSubscribedToPostEventDto(string UserId, int SubscriptionId, int PostId);

    public record UserUnSubscribedToForumEventDto(string UserId, int SubscriptionId);

    public record UserUnSubscribedToPostEventDto(string UserId, int SubscriptionId);
}
