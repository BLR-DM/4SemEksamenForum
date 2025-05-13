using ContentService.Application.Commands.CommandDto.PostDto;
using ContentService.Application.Commands.Interfaces;
using ContentService.Application.Queries;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ContentService.Api.Endpoints
{
    public static class PostEndpoints
    {
        public static void MapPostEndpoints(this WebApplication app)
        {
            const string tag = "Post";

            // Query
            app.MapGet("/forum/{forumId}/post/{postId}",
                async (IForumQuery query, int forumId, int postId) =>
                {
                    var result = await query.GetForumWithSinglePostAsync(forumId, postId);
                    return Results.Ok(result);
                }).WithTags(tag);

            app.MapGet("/forums/{forumName}/post/{postId}",
                async (IForumQuery query, string forumName, int postId) =>
                {
                    var result = await query.GetForumByNameWithSinglePostAsync(forumName, postId);
                    return Results.Ok(result);
                }).WithTags(tag);

            // Write
            app.MapPost("/forum/{forumId}/post",
                async (IForumCommand command, CreatePostDto postDto, int forumId, ClaimsPrincipal user) =>
                {
                    var appUserId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    var username = user.FindFirst("preferred_username")?.Value;

                    await command.CreatePostAsync(postDto, username, appUserId, forumId);
                    return Results.Created();
                }).WithTags(tag);

            app.MapPut("/forum/{forumId}/post/{postId}",
                async (IForumCommand command, UpdatePostDto postDto, string appUserId, int forumId, int postId) =>
                {
                    await command.UpdatePostAsync(postDto, appUserId, forumId, postId);
                    return Results.Ok(postDto);
                }).WithTags(tag);

            app.MapDelete("/forum/{forumId}/post/{postId}",
                async (IForumCommand command, string appUserId, int forumId, int postId) =>
                {
                    await command.DeletePostAsync(appUserId, forumId, postId);
                    return Results.Ok("Post deleted");
                }).WithTags(tag);
        }
    }
}
