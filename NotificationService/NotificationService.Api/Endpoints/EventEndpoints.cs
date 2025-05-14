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

            app.MapPost($"/events/{EventNames.PostPublished}", async (PostPublishedDto dto, INotificatioHandler notificationHandler) =>
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

        }
    }
}