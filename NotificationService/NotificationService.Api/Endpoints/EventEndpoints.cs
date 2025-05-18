using NotificationService.Application.Commands.CommandDto;
using NotificationService.Application.Commands.Interfaces;
using NotificationService.Application.EventDtos;
using NotificationService.Application.Helpers;
using NotificationService.Application.Services;

namespace NotificationService.Api.Endpoints
{
    public static class EventEndpoints
    {
        public static void MapEventEndpoints(this WebApplication app)
        {
            const string tag = "Events";

            app.MapPost($"/events/{EventNames.PostPublished}", async (PostPublishedDto dto, INotificationHandler notificationHandler) =>
            {
                try
                {
                    await notificationHandler.Handle(EventNames.PostPublished, dto);

                    return Results.Ok();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return Results.Problem();
                }
            }).WithTopic("pubsub", EventNames.PostPublished).AllowAnonymous();

            app.MapPost($"/events/{EventNames.RequestedForumSubscribersCollected}", async (RequestedForumSubscribersCollectedEventDto dto, ISentNotificationCommand command) =>
            {
                try
                {
                    foreach (var userId in dto.UserIds)
                    {
                        await command.CreateSentNotificationAsync(new CreateSentNotificationDto(dto.NotificationId, userId));
                    }

                    return Results.Ok();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return Results.Problem();
                }
            }).WithTopic("pubsub", EventNames.RequestedForumSubscribersCollected).AllowAnonymous();



            app.MapPost($"/events/{EventNames.CommentPublished}", async (CommentPublishedDto dto, INotificationHandler notificationHandler) =>
            {
                try
                {
                    await notificationHandler.Handle(EventNames.CommentPublished, dto);

                    return Results.Ok();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return Results.Problem();
                }
            }).WithTopic("pubsub", EventNames.CommentPublished).AllowAnonymous();


            app.MapPost($"/events/{EventNames.RequestedPostSubscribersCollected}", async (RequestedPostSubscribersCollectedEventDto dto, ISentNotificationCommand command) =>
            {
                try
                {
                    foreach (var userId in dto.UserIds)
                    {
                        await command.CreateSentNotificationAsync(new CreateSentNotificationDto(dto.NotificationId, userId));
                    }

                    return Results.Ok();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return Results.Problem();
                }
            }).WithTopic("pubsub", EventNames.RequestedPostSubscribersCollected).AllowAnonymous();


            app.MapPost($"/events/{EventNames.PostVoteCreated}", async (PostVoteEventDto dto, INotificationHandler notificationHandler) =>
            {
                try
                {
                    await notificationHandler.Handle(EventNames.PostVoteCreated, dto);

                    return Results.Ok();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return Results.Problem();
                }
            }).WithTopic("pubsub", EventNames.PostUpVoteCreated).WithTopic("pubsub", EventNames.PostDownVoteCreated).AllowAnonymous();
        }
    }
}