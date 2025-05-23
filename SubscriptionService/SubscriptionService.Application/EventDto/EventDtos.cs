﻿namespace SubscriptionService.Application.EventDto
{
    public record UserSubscribedToForumEventDto(string UserId, int SubscriptionId, int ForumId);

    public record FailedToSubscribeUserToForumEventDto(string UserId, int ForumId);
    public record FailedToSubscribeUserToPostEventDto(string UserId, int ForumId, int PostId);

    public record FailedToSubscribeUserToPostOnCommentEventDto(string UserId, int ForumId, int PostId, int CommentId);

    public record UserSubscribedToPostEventDto(string UserId, int SubscriptionId, int PostId);

    public record UserUnSubscribedFromForumEventDto(string UserId, int SubscriptionId, int ForumId);

    public record UserUnSubscribedFromPostEventDto(string UserId, int SubscriptionId, int PostId);

    public record NotifyForumSubscriberEventDto(int ForumId, int PostId);

    public record PostPublishedDto(string UserId, int ForumId, int PostId);

    public record NotifyPostSubscriberEventDto(int ForumId, int PostId);

    public record CommentPublishedDto(string UserId, int ForumId, int PostId, int CommentId, string Title);

    public record SubscriberNotificationEventDto(string UserId, int ForumId, int PostId);

    public record ForumSubscribersRequestedEventDto(int NotificationId, int ForumId);
    public record PostSubscribersRequestedEventDto(int NotificationId, int PostId);

    public record RequestedForumSubscribersCollectedEventDto(IEnumerable<string> UserIds, int NotificationId);
}
