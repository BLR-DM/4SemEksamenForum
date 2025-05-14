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
            ///// Endpoint verbs forum/... or need to configure CloudEvents payload etc.
            //
            //  MapGroup remove forum prefix

            // Query
            app.MapGet("/forum",
                async (IForumQuery query) =>
                {
                    var result = await query.GetForumsAsync();
                    return Results.Ok(result);
                }).WithTags(tag);

            app.MapGet("/forum/{forumId}",
                async (IForumQuery query, int forumId) =>
                {
                    var result = await query.GetForumAsync(forumId);
                    return Results.Ok(result);
                }).WithTags(tag);

            //app.MapGet("/forum/{forumId}/posts",
            //    async (IForumQuery query, int forumId) =>
            //    {
            //        var result = await query.GetForumWithPostsAsync(forumId);
            //        return Results.Ok(result);
            //    }).WithTags(tag);

            app.MapGet("/forum/{forumName}/posts",
                async (IForumQuery query, string forumName) =>
                {
                    var result = await query.GetForumByNameWithPostsAsync(forumName);
                    return Results.Ok(result);
                }).WithTags(tag);

            // Write
            app.MapPost("/forum",
                async (IForumCommand command, CreateForumDto forumDto, ClaimsPrincipal user) =>
                {
                    var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                    await command.CreateForumAsync(forumDto, userId);
                    return Results.Created();
                }).WithTags(tag).RequireAuthorization();

            app.MapPut("/forum/{forumId}",
                async (IForumCommand command, UpdateForumDto forumDto, ClaimsPrincipal user, int forumId) =>
                {
                    var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    await command.UpdateForumAsync(forumDto, userId, forumId);
                    return Results.Ok(forumDto);
                }).WithTags(tag);

            app.MapDelete("/forum/{forumId}", // check appUserId / moderator
                async (IForumCommand command, ClaimsPrincipal user, int forumId) =>
                {
                    var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    await command.DeleteForumAsync(userId, forumId);
                    return Results.NoContent();
                }).WithTags(tag);

            //app.MapPost("/forum/approved",
            //    async (IForumCommand command, PublishForumDto forumDto) =>
            //    {
            //        await command.HandleForumApprovalAsync(forumDto);
            //        return Results.Ok();
            //    }).WithTopic("pubsub", "forumApproved");

            //app.MapPost("/forum/publish",
            //    async (IForumCommand command, PublishForumDto forumDto) =>
            //    {
            //        await command.HandlePublishAsync(forumDto);
            //        return Results.Ok();
            //    }).WithTopic("pubsub", "forumToPublish");
        }
    }
}
