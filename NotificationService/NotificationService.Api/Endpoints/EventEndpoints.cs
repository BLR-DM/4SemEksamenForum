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
            }).WithTopic("pubsub", EventNames.PostPublished);

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
            }).WithTopic("pubsub", EventNames.RequestedForumSubscribersCollected);



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
            }).WithTopic("pubsub", EventNames.CommentPublished);


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
            }).WithTopic("pubsub", EventNames.RequestedPostSubscribersCollected);


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
            }).WithTopic("pubsub", EventNames.PostUpVoteCreated).WithTopic("pubsub", EventNames.PostDownVoteCreated);

            app.MapPost($"/events/{EventNames.PostRejected}", async (PostRejectedDto dto, INotificationHandler notificationHandler) =>
            {
                try
                {
                    await notificationHandler.Handle(EventNames.PostRejected, dto);

                    return Results.Ok();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return Results.Problem();
                }
            }).WithTopic("pubsub", EventNames.PostRejected);

            app.MapPost($"/events/{EventNames.ForumRejected}", async (ForumRejectedDto dto, INotificationHandler notificationHandler) =>
            {
                try
                {
                    await notificationHandler.Handle(EventNames.ForumRejected, dto);

                    return Results.Ok();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return Results.Problem();
                }
            }).WithTopic("pubsub", EventNames.ForumRejected);

            app.MapPost($"/events/{EventNames.CommentRejected}", async (CommentRejectedDto dto, INotificationHandler notificationHandler) =>
            {
                try
                {
                    await notificationHandler.Handle(EventNames.CommentRejected, dto);

                    return Results.Ok();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return Results.Problem();
                }
            }).WithTopic("pubsub", EventNames.CommentRejected);
        }


    }
}