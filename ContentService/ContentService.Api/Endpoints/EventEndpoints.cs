using ContentService.Application.Commands.Interfaces;
using ContentService.Application.EventDto;
using ContentService.Application.Services;

namespace ContentService.Api.Endpoints
{
    public static class EventEndpoints
    {
        public static void MapEventEndpoints(this WebApplication app)
        {
            var events = app.MapGroup("/events"); //.RequireAuthorization("Internal")

            events.MapPost("/content-moderated",
                    async (IModerationResultHandler handler, ContentModeratedDto dto) =>
                    {
                        try
                        {
                            Console.WriteLine($"Received moderation result: {dto.ContentId} = {dto.Result}");
                            await handler.HandleModerationResultAsync(dto);
                            return Results.Ok();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            return Results.Problem(ex.Message);
                        }
                    })
                .WithTopic("pubsub", "content-moderated");


            events.MapPost("/compensate/delete-forum",
                    async (IForumCommand command, CompensateByDeletingForumDto evt) =>
                    {
                        try
                        {
                            await command.DeleteForumAsync(evt.UserId, evt.ForumId);
                            return Results.NoContent();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            return Results.Problem(ex.Message);
                        }
                    })
                .WithTopic("pubsub", "failed-to-subscribe-user-on-forum-published")
                .WithTopic("pubsub", "failed-to-add-points-on-forum-published");


            events.MapPost("/compensate/delete-post",
                    async (IForumCommand command, CompensateByDeletingPostDto evt) =>
                    {
                        try
                        {
                            await command.DeletePostAsync(evt.UserId, evt.ForumId, evt.PostId);
                            return Results.NoContent();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            return Results.Problem(ex.Message);
                        }
                    })
                .WithTopic("pubsub", "failed-to-subscribe-user-on-post-published")
                .WithTopic("pubsub", "failed-to-add-points-on-post-published");

            events.MapPost("/compensate/delete-comment",
                    async (IPostCommand command, CompensateByDeletingCommentDto evt) =>
                    {
                        try
                        {
                            await command.DeleteCommentAsync(evt.UserId, evt.ForumId, evt.PostId, evt.CommentId);
                            return Results.NoContent();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            return Results.Problem(ex.Message);
                        }
                    })
                .WithTopic("pubsub", "failed-to-add-points-on-comment-published")
                .WithTopic("pubsub", "failed-to-subscribe-user-on-comment-published");
        }
    }
}
