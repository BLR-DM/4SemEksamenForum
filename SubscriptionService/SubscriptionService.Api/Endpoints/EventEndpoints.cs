using SubscriptionService.Application.Commands.Interfaces;
using SubscriptionService.Application.EventDto;
using SubscriptionService.Application.Queries.Interfaces;
using SubscriptionService.Application.Services;

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

            app.MapPost("/events/post-published",
                async (PostPublishedDto evtDto, IForumSubCommand forumSubCommand, IPostSubCommand postSubCommand) =>
                {
                    try
                    {
                        await forumSubCommand.CreateAsync(evtDto.ForumId, evtDto.UserId);
                        await postSubCommand.CreateAsync(evtDto.PostId, evtDto.UserId);

                        return Results.Ok();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        return Results.Problem(ex.Message);
                    }
                }).WithTopic("pubsub", "post-published");

            app.MapPost("/events/forum-published",
                async (ForumPublishedDto evtDto, IForumSubCommand command) =>
                {
                    try
                    {
                        await command.CreateAsync(evtDto.ForumId, evtDto.UserId);

                        return Results.Ok();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        return Results.Problem(ex.Message);
                    }
                }).WithTopic("pubsub", "forum-published");
        }
    }
}