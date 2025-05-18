using System.Security.Claims;
using PointService.Application.Command;
using PointService.Application.Command.CommandDto;
using PointService.Application.EventDto;
using PointService.Application.Queries;
using PointService.Application.Services;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace PointService.Api.Endpoints
{
    public static class EventEndpoints
    {
        public static void MapEventEndpoints(this WebApplication app)
        {
            const string tag = "Events";

            app.MapPost("/events/forum-published", async (ForumPublishedDto forumPublishedDto, IPointEntryCommand command,
            IEventHandler eventHandler) =>
            {
                try
                {
                    await command.CreatePointEntryAsync(new CreatePointEntryDto
                    {
                        PointActionId = "forum-published",
                        SourceId = forumPublishedDto.ForumId,
                        SourceType = "Forum",
                        ContextId = forumPublishedDto.ForumId,
                        ContextType = "Forum"
                    }, forumPublishedDto.UserId);

                    return Results.Ok();
                }
                catch (Exception ex)
                {
                    await eventHandler.FailedToAddPointsOnForumPublished(forumPublishedDto.UserId, forumPublishedDto.ForumId);
                    Console.WriteLine(ex.Message);
                    return Results.Problem();
                }

            }).WithTopic("pubsub", "forum-published");

            app.MapPost("/events/forum-deleted", async (ForumDeletedDto forumDeletedDto, IPointEntryCommand command, IPointEntryQuery query) =>
            {
                try
                {
                    var hasUserRecievedPointsUponCreation = await query.ExistsAsync(forumDeletedDto.UserId,
                        "forum-published", forumDeletedDto.ForumId, "Forum");

                    if(!hasUserRecievedPointsUponCreation)
                    {
                        return Results.Ok();
                    }

                    await command.CreatePointEntryAsync(new CreatePointEntryDto
                    {
                        PointActionId = "forum-deleted",
                        SourceId = forumDeletedDto.ForumId,
                        SourceType = "Forum",
                        ContextId = forumDeletedDto.ForumId,
                        ContextType = "Forum"
                    }, forumDeletedDto.UserId);

                    return Results.Ok();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return Results.Problem();
                }
            });

            app.MapPost("/events/post-published", async (PostPublishedDto postPublishedDto, IPointEntryCommand command,
                IEventHandler eventHandler) =>
            {
                try
                {
                    // Check if user received points for creating this post

                    await command.CreatePointEntryAsync(new CreatePointEntryDto
                    {
                        PointActionId = "post-published",
                        SourceId = postPublishedDto.ForumId,
                        SourceType = "Forum",
                        ContextId = postPublishedDto.PostId,
                        ContextType = "Post"
                    }, postPublishedDto.UserId);

                    return Results.Ok();
                }
                catch (Exception ex)
                {
                    await eventHandler.FailedToAddPointsOnPostPublished(
                        postPublishedDto.UserId, postPublishedDto.ForumId, postPublishedDto.PostId);
                    Console.WriteLine(ex.Message);
                    return Results.Problem();
                }


            }).WithTopic("pubsub", "post-published");

            app.MapPost("/events/post-deleted", async (PostDeletedDto postDeletedDto, IPointEntryCommand command, IPointEntryQuery query) =>
            {
                try
                {
                    var hasUserRecievedPointsUponCreation = await query.ExistsAsync(postDeletedDto.UserId,
                        "post-published", postDeletedDto.PostId, "Post");

                    if (!hasUserRecievedPointsUponCreation)
                    {
                        return Results.Ok();
                    }

                    await command.CreatePointEntryAsync(new CreatePointEntryDto
                    {
                        PointActionId = "post-deleted",
                        SourceId = postDeletedDto.ForumId,
                        SourceType = "Forum",
                        ContextId = postDeletedDto.PostId,
                        ContextType = "Post"
                    }, postDeletedDto.UserId);

                    return Results.Ok();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return Results.Problem();
                }
            }).WithTopic("pubsub", "post-deleted");

            app.MapPost("/events/comment-published", async (CommentPublishedDto commentPublishedDto, IPointEntryCommand command) =>
            {
                try
                {
                    await command.CreatePointEntryAsync(new CreatePointEntryDto
                    {
                        PointActionId = "comment-published",
                        SourceId = commentPublishedDto.PostId,
                        SourceType = "Post",
                        ContextId = commentPublishedDto.CommentId,
                        ContextType = "Comment"
                    }, commentPublishedDto.UserId);

                    return Results.Ok();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return Results.Problem();
                }


            }).WithTopic("pubsub", "comment-published");

            app.MapPost("/events/comment-deleted", async (CommentDeletedDto commentDeletedDto, IPointEntryCommand command, IPointEntryQuery query) =>
            {
                try
                {
                    var hasUserRecievedPointsUponCreation = await query.ExistsAsync(commentDeletedDto.UserId,
                        "comment-published", commentDeletedDto.CommentId, "Comment");

                    if (!hasUserRecievedPointsUponCreation)
                    {
                        return Results.Ok();
                    }

                    await command.CreatePointEntryAsync(new CreatePointEntryDto
                    {
                        PointActionId = "comment-deleted",
                        SourceId = commentDeletedDto.PostId,
                        SourceType = "Post",
                        ContextId = commentDeletedDto.CommentId,
                        ContextType = "Comment"
                    }, commentDeletedDto.UserId);

                    return Results.Ok();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return Results.Problem();
                }
            }).WithTopic("pubsub", "comment-deleted");

            app.MapPost("/events/user-subscribed-to-forum", async (UserSubscribedToForumEventDto userSubscribedToForumEventDto, IPointEntryCommand command) =>
            {
                try
                {
                    await command.CreatePointEntryAsync(new CreatePointEntryDto
                    {
                        PointActionId = "user-subscribed-to-forum",
                        SourceId = userSubscribedToForumEventDto.ForumId,
                        SourceType = "Forum",
                        ContextId = userSubscribedToForumEventDto.SubscriptionId,
                        ContextType = "Subscription"
                    }, userSubscribedToForumEventDto.UserId);

                    return Results.Ok();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return Results.Problem();
                }
            }).WithTopic("pubsub", "user-subscribed-to-forum");

            app.MapPost("/events/user-unsubscribed-from-forum", async (UserUnSubscribedFromForumEventDto userUnSubscribedFromForumEventDto, IPointEntryCommand command, IPointEntryQuery query) =>
            {
                try
                {
                    var hasUserRecievedPointsUponCreation = await query.ExistsAsync(userUnSubscribedFromForumEventDto.UserId,
                        "user-subscribed-to-forum", userUnSubscribedFromForumEventDto.SubscriptionId, "Subscription");

                    if (!hasUserRecievedPointsUponCreation)
                    {
                        return Results.Ok();
                    }


                    await command.CreatePointEntryAsync(new CreatePointEntryDto
                    {
                        PointActionId = "user-unsubscribed-from-forum",
                        SourceId = userUnSubscribedFromForumEventDto.ForumId,
                        SourceType = "Forum",
                        ContextId = userUnSubscribedFromForumEventDto.SubscriptionId,
                        ContextType = "Subscription"
                    }, userUnSubscribedFromForumEventDto.UserId);

                    return Results.Ok();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return Results.Problem();
                }
            }).WithTopic("pubsub", "user-unsubscribed-from-forum");

            app.MapPost("/events/user-subscribed-to-post", async (UserSubscribedToPostEventDto userSubscribedToPostEventDto, IPointEntryCommand command) =>
            {
                try
                {
                    await command.CreatePointEntryAsync(new CreatePointEntryDto
                    {
                        PointActionId = "user-subscribed-to-post",
                        SourceId = userSubscribedToPostEventDto.PostId,
                        SourceType = "Post",
                        ContextId = userSubscribedToPostEventDto.SubscriptionId,
                        ContextType = "Subscription"
                    }, userSubscribedToPostEventDto.UserId);

                    return Results.Ok();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return Results.Problem();
                }
            }).WithTopic("pubsub", "user-subscribed-to-post");

            app.MapPost("/events/user-unsubscribed-from-post", async (UserUnSubscribedFromPostEventDto userUnSubscribedFromPostEventDto, IPointEntryCommand command, IPointEntryQuery query) =>
            {
                try
                {
                    var hasUserRecievedPointsUponCreation = await query.ExistsAsync(userUnSubscribedFromPostEventDto.UserId,
                        "user-subscribed-to-post", userUnSubscribedFromPostEventDto.SubscriptionId, "Subscription");

                    if (!hasUserRecievedPointsUponCreation)
                    {
                        return Results.Ok();
                    }


                    await command.CreatePointEntryAsync(new CreatePointEntryDto
                    {
                        PointActionId = "user-unsubscribed-from-post",
                        SourceId = userUnSubscribedFromPostEventDto.PostId,
                        SourceType = "Post",
                        ContextId = userUnSubscribedFromPostEventDto.SubscriptionId,
                        ContextType = "Subscription"
                    }, userUnSubscribedFromPostEventDto.UserId);

                    return Results.Ok();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return Results.Problem();
                }
            }).WithTopic("pubsub", "user-unsubscribed-from-post");

            app.MapPost("/events/post-upvote-created", async (PostVoteEventDto postVoteEventDto, IPointEntryCommand command) =>
            {
                try
                {
                    await command.CreatePointEntryAsync(new CreatePointEntryDto
                    {
                        PointActionId = "post-upvote-created",
                        SourceId = postVoteEventDto.PostId,
                        SourceType = "Post",
                        ContextId = 0,
                        ContextType = "NoVoteId"
                    }, postVoteEventDto.UserId);

                    return Results.Ok();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return Results.Problem();
                }
            }).WithTopic("pubsub", "post-upvote-created");

            app.MapPost("/events/post-upvote-deleted", async (PostVoteEventDto postVoteEventDto, IPointEntryCommand command) =>
            {
                try
                {
                    await command.CreatePointEntryAsync(new CreatePointEntryDto
                    {
                        PointActionId = "post-upvote-deleted",
                        SourceId = postVoteEventDto.PostId,
                        SourceType = "Post",
                        ContextId = 0,
                        ContextType = "NoVoteId"
                    }, postVoteEventDto.UserId);

                    return Results.Ok();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return Results.Problem();
                }
            }).WithTopic("pubsub", "post-upvote-deleted");

            app.MapPost("/events/comment-upvote-created", async (CommentVoteEventDto commentVoteEventDto, IPointEntryCommand command) =>
            {
                try
                {
                    await command.CreatePointEntryAsync(new CreatePointEntryDto
                    {
                        PointActionId = "comment-upvote-created",
                        SourceId = commentVoteEventDto.CommentId,
                        SourceType = "Comment",
                        ContextId = 0,
                        ContextType = "NoVoteId"
                    }, commentVoteEventDto.UserId);

                    return Results.Ok();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return Results.Problem();
                }
            }).WithTopic("pubsub", "comment-upvote-created");

            app.MapPost("/events/comment-upvote-deleted", async (CommentVoteEventDto commentVoteEventDto, IPointEntryCommand command) =>
            {
                try
                {
                    await command.CreatePointEntryAsync(new CreatePointEntryDto
                    {
                        PointActionId = "comment-upvote-deleted",
                        SourceId = commentVoteEventDto.CommentId,
                        SourceType = "Comment",
                        ContextId = 0,
                        ContextType = "NoVoteId"
                    }, commentVoteEventDto.UserId);

                    return Results.Ok();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return Results.Problem();
                }
            }).WithTopic("pubsub", "comment-upvote-deleted");

        }
    }
}
