using SubscriptionService.Application.Commands.Interfaces;
using SubscriptionService.Application.EventDto;
using SubscriptionService.Application.Queries.Interfaces;
using SubscriptionService.Application.Services;
using EventHandler = SubscriptionService.Application.Services.EventHandler;

namespace SubscriptionService.Api.Endpoints
{
    public static class EventEndpoints
    {
        public static void MapEventEndpoints(this WebApplication app)
        {
            const string tag = "Events";

            app.MapPost("/events/forum-subscribers-requested",
                async (ForumSubscribersRequestedEventDto evtDto, IForumSubQuery query, IEventHandler eventHandler) =>
                {
                    var userIds = await query.GetSubscriptionsByForumIdAsync(evtDto.ForumId);
                    await eventHandler.RequestedForumSubscribersCollected(userIds, evtDto.NotificationId);

                }).WithTopic("pubsub", "forum-subscribers-requested");

            app.MapPost("/events/post-subscribers-requested",
                async (PostSubscribersRequestedEventDto evtDto, IPostSubQuery query, IEventHandler eventHandler) =>
                {
                    var userIds = await query.GetSubscriptionsByPostIdAsync(evtDto.PostId);
                    await eventHandler.RequestedPostSubscribersCollected(userIds, evtDto.NotificationId);

                }).WithTopic("pubsub", "post-subscribers-requested");


            app.MapPost("/events/post-published",
                async (PostPublishedDto evtDto, IForumSubCommand forumSubCommand, IPostSubCommand postSubCommand,
                    IEventHandler eventHandler) =>
                {
                    try
                    {
                        await forumSubCommand.CreateAsync(evtDto.ForumId, evtDto.UserId);
                        await postSubCommand.CreateAsync(evtDto.PostId, evtDto.UserId);

                        return Results.Ok();
                    }
                    catch (Exception ex)
                    {
                        await eventHandler.FailedToSubscribeUserOnPostPublished(evtDto.UserId, evtDto.ForumId, evtDto.PostId);
                        Console.WriteLine(ex.Message);
                        return Results.Problem(ex.Message);
                    }
                }).WithTopic("pubsub", "post-published");

            app.MapPost("/events/comment-published",
                async (CommentPublishedDto evtDto, IPostSubCommand postSubCommand) =>
                {
                    try
                    {
                        await postSubCommand.CreateAsync(evtDto.PostId, evtDto.UserId);

                        return Results.Ok();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        return Results.Problem(ex.Message);
                    }
                }).WithTopic("pubsub", "comment-published");

            app.MapPost("/events/forum-published",
                async(ForumPublishedDto evtDto, IForumSubCommand command, IEventHandler eventHandler) =>
                {
                    try
                    {
                        await command.CreateAsync(evtDto.ForumId, evtDto.UserId);

                        return Results.Ok();
                    }
                    catch (Exception ex)
                    {
                        await eventHandler.FailedToSubscribeUserOnForumPublished(evtDto.UserId, evtDto.ForumId);
                        Console.WriteLine(ex.Message);
                        return Results.Problem(ex.Message);
                    }
                }).WithTopic("pubsub", "forum-published");
        }
    }
}