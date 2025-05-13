using System.Security.Claims;
using PointService.Application.Command;
using PointService.Application.Command.CommandDto;
using PointService.Application.EventDto;

namespace PointService.Api.Endpoints
{
    public static class EventEndpoints
    {
        public static void MapEventEndpoints(this WebApplication app)
        {
            const string tag = "Events";

            app.MapPost("/forum-published", async (ForumPublishedDto forumPublishedDto, IPointEntryCommand command) =>
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
                    Console.WriteLine(ex.Message);
                    return Results.Problem();
                }

            }).WithTopic("pubsub", "forum-published").AllowAnonymous();

            app.MapPost("/forum-deleted", async (IPointEntryCommand command) =>
            {
                // MANGER AT PUBLISH USERID PÅ FORUMMET
            });

            app.MapPost("/post-published", async (PostPublishedDto postPublishedDto, IPointEntryCommand command) =>
            {
                try
                {
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
                    Console.WriteLine(ex.Message);
                    return Results.Problem();
                }


            }).WithTopic("pubsub", "post-published").AllowAnonymous();

            app.MapPost("/post-deleted", async (IPointEntryCommand command) =>
            {
                 // MANGER AT PUBLISH USERID PÅ POSTET
            });

            app.MapPost("/comment-published", async (CommentPublishedDto commentPublishedDto, IPointEntryCommand command) =>
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


            }).WithTopic("pubsub", "comment-published").AllowAnonymous();

            app.MapPost("/comment-deleted", async (IPointEntryCommand command) =>
            {
                // MANGER AT PUBLISH USERID PÅ COMMENT
            });

        }
    }
}
