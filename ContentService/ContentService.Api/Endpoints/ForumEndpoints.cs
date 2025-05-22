using System.Security.Claims;
using ContentService.Application.Commands.CommandDto.ForumDto;
using ContentService.Application.Commands.CommandDto.PostDto;
using ContentService.Application.Commands.Interfaces;
using ContentService.Application.Queries;

namespace ContentService.Api.Endpoints
{
    public static class ForumEndpoints
    {
        public static void MapForumEndpoints(this WebApplication app)
        {
            const string tag = "Forum";

            // Query
            app.MapGet("/api/forum",
                async (IForumQuery query) =>
                {
                    try
                    {
                        var result = await query.GetForumsAsync();
                        return Results.Ok(result);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        return Results.Problem(ex.Message);
                    }
                }).WithTags(tag).RequireAuthorization("StandardUser");

            app.MapGet("/api/forum/{forumId}",
                async (IForumQuery query, int forumId) =>
                {
                    try
                    {
                        var result = await query.GetForumAsync(forumId);
                        return Results.Ok(result);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        return Results.Problem(ex.Message);
                    }
                }).WithTags(tag).RequireAuthorization("StandardUser");

            app.MapGet("/api/forums/posts",
                async (IForumQuery query) =>
                {
                    try
                    {
                        var result = await query.GetForumsWithPostsAsync();
                        return Results.Ok(result);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        return Results.Problem(ex.Message);
                    }
                }).WithTags(tag).RequireAuthorization("StandardUser");

            app.MapGet("/api/forum/{forumName}/posts",
                async (IForumQuery query, string forumName) =>
                {
                    try
                    {
                        var result = await query.GetForumByNameWithPostsAsync(forumName);
                        return Results.Ok(result);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        return Results.Problem(ex.Message);
                    }
                }).WithTags(tag).RequireAuthorization("StandardUser");

            // Write
            app.MapPost("/api/forum",
                async (IForumCommand command, CreateForumDto forumDto, ClaimsPrincipal user) =>
                {
                    try
                    {
                        var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                        await command.CreateForumAsync(forumDto, userId);
                        return Results.Created();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        return Results.Problem(ex.Message);
                    }
                }).WithTags(tag).RequireAuthorization("StandardUser");

            app.MapPut("/api/forum/{forumId}",
                async (IForumCommand command, UpdateForumDto forumDto, ClaimsPrincipal user, int forumId) =>
                {
                    try
                    {
                        var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                        await command.UpdateForumAsync(forumDto, userId, forumId);
                        return Results.Ok(forumDto);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        return Results.Problem(ex.Message);
                    }
                }).WithTags(tag).RequireAuthorization("StandardUser");

            app.MapDelete("/api/forum/{forumId}",
                async (IForumCommand command, ClaimsPrincipal user, int forumId) =>
                {
                    try
                    {
                        if (user.IsInRole("moderator"))
                        {
                            await command.DeleteForumModAsync(forumId);
                            return Results.NoContent();
                        }
                        var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                        await command.DeleteForumAsync(userId, forumId);
                        return Results.NoContent();
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
