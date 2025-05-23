﻿using ContentService.Application.Commands.CommandDto.PostDto;
using ContentService.Application.Commands.Interfaces;
using ContentService.Application.Queries;
using System.Security.Claims;

namespace ContentService.Api.Endpoints
{
    public static class PostEndpoints
    {
        public static void MapPostEndpoints(this WebApplication app)
        {
            const string tag = "Post";

            // Query
            app.MapGet("/api/forum/{forumId}/post/{postId}",
                async (IForumQuery query, int forumId, int postId) =>
                {
                    try
                    {
                        var result = await query.GetForumWithSinglePostAsync(forumId, postId);
                        return Results.Ok(result);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        return Results.Problem(ex.Message);
                    }
                }).WithTags(tag).RequireAuthorization("StandardUser");

            app.MapGet("/api/forums/{forumName}/post/{postId}",
                async (IForumQuery query, string forumName, int postId) =>
                {
                    try
                    {
                        var result = await query.GetForumByNameWithSinglePostAsync(forumName, postId);
                        return Results.Ok(result);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        return Results.Problem(ex.Message);
                    }
                }).WithTags(tag).RequireAuthorization("StandardUser");

            // Write
            app.MapPost("/api/forum/{forumId}/post",
                async (IForumCommand command, CreatePostDto postDto, int forumId, ClaimsPrincipal user) =>
                {
                    try
                    {
                        var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                        var username = user.FindFirst("preferred_username")?.Value;

                        await command.CreatePostAsync(postDto, username, userId, forumId);
                        return Results.Created();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        return Results.Problem(ex.Message);
                    }
                }).WithTags(tag).RequireAuthorization("StandardUser");

            app.MapPut("/api/forum/{forumId}/post/{postId}",
                async (IForumCommand command, UpdatePostDto postDto, ClaimsPrincipal user, int forumId, int postId) =>
                {
                    try
                    {
                        var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                        await command.UpdatePostAsync(postDto, userId, forumId, postId);
                        return Results.Ok(postDto);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        return Results.Problem(ex.Message);
                    }
                }).WithTags(tag).RequireAuthorization("StandardUser");

            app.MapDelete("/api/forum/{forumId}/post/{postId}",
                async (IForumCommand command, int forumId, int postId, ClaimsPrincipal user) =>
                {
                    try
                    {
                        if (user.IsInRole("moderator"))
                        {
                            await command.DeletePostModAsync(forumId, postId);
                            return Results.Ok("Post deleted");
                        }
                        var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                        await command.DeletePostAsync(userId, forumId, postId);
                        return Results.Ok("Post deleted");
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
