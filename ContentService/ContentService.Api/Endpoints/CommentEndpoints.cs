using System.Security.Claims;
using ContentService.Application.Commands.CommandDto.CommentDto;
using ContentService.Application.Commands.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ContentService.Api.Endpoints
{
    public static class CommentEndpoints
    {
        public static void MapCommentEndpoints(this WebApplication app)
        {
            const string tag = "Comment";

            app.MapPost("/forum/{forumId}/post/{postId}/comment", 
                async (IPostCommand command, CreateCommentDto commentDto, int forumId, int postId, ClaimsPrincipal user) =>
                {
                    var appUserId = user.FindFirstValue(ClaimTypes.NameIdentifier);
                    var username = user.FindFirstValue("preferred_username");

                    await command.CreateCommentAsync(commentDto, username, postId, appUserId, forumId);
                    return Results.Created();
                }).WithTags(tag);

            app.MapPut("/forum/{forumId}/post/{postId}/comment/{commentId}",
                async (IPostCommand command, UpdateCommentDto commentDto, int forumId, int postId, int commentId, ClaimsPrincipal user) =>
                {
                    var appUserId = user.FindFirstValue(ClaimTypes.NameIdentifier);

                    await command.UpdateCommentAsync(commentDto, appUserId, forumId, postId, commentId);
                    return Results.Ok(commentDto);
                }).WithTags(tag);

            app.MapDelete("/forum/{forumId}/post/{postId}/comment/{commentId}",
                async (IPostCommand command, int forumId, int postId, int commentId, ClaimsPrincipal user) =>
                {
                    var appUserId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                    await command.DeleteCommentAsync(appUserId, forumId, postId, commentId);
                    return Results.Ok("Comment deleted");
                }).WithTags(tag);
        }
    }
}
