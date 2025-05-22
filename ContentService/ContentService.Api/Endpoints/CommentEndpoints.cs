using System.Security.Claims;
using ContentService.Application.Commands.CommandDto.CommentDto;
using ContentService.Application.Commands.Interfaces;

namespace ContentService.Api.Endpoints
{
    public static class CommentEndpoints
    {
        public static void MapCommentEndpoints(this WebApplication app)
        {
            const string tag = "Comment";

            app.MapPost("/api/forum/{forumId}/post/{postId}/comment", 
                async (IPostCommand command, CreateCommentDto commentDto, int forumId, int postId, ClaimsPrincipal user) =>
                {
                    try
                    {
                        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
                        var username = user.FindFirstValue("preferred_username");

                        await command.CreateCommentAsync(commentDto, username, postId, userId, forumId);
                        return Results.Created();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        return Results.Problem(ex.Message);
                    }
                }).WithTags(tag).RequireAuthorization("StandardUser");

            app.MapPut("/api/forum/{forumId}/post/{postId}/comment/{commentId}",
                async (IPostCommand command, UpdateCommentDto commentDto, int forumId, int postId, int commentId, ClaimsPrincipal user) =>
                {
                    try
                    {
                        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);

                        await command.UpdateCommentAsync(commentDto, userId, forumId, postId, commentId);
                        return Results.Ok(commentDto);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        return Results.Problem(ex.Message);
                    }
                }).WithTags(tag).RequireAuthorization("StandardUser");

            app.MapDelete("/api/forum/{forumId}/post/{postId}/comment/{commentId}",
                async (IPostCommand command, int forumId, int postId, int commentId, ClaimsPrincipal user) =>
                {
                    try
                    {
                        if (user.IsInRole("moderator"))
                        {
                            await command.DeleteCommentModAsync(forumId, postId, commentId);
                            return Results.Ok("Comment deleted");
                        }
                        var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                        await command.DeleteCommentAsync(userId, forumId, postId, commentId);
                        return Results.Ok("Comment deleted");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        return Results.Problem(ex.Message);
                    }
                }).WithTags(tag).RequireAuthorization("StandardUser");
        }
    }
}
