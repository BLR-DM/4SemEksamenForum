using Dapr;
using Dapr.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;
using NotificationService.Application;
using NotificationService.Application.Commands.CommandDto;
using NotificationService.Application.Commands.Interfaces;
using NotificationService.Application.EventDtos;
using NotificationService.Application.Factories.Interfaces;
using NotificationService.Application.Queries;
using NotificationService.Application.Services;
using NotificationService.Infrastructure;
using System.Text.Json;
using static Google.Rpc.Context.AttributeContext.Types;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

builder.Services.AddDaprClient();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "https://keycloak.blrforum.dk/realms/4SemForumProjekt";
        options.Audience = "notificationservice-api";
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowGateway", builder =>
    {
        builder.WithOrigins("http://localhost:5000")
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

//app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.UseCloudEvents();
app.MapSubscribeHandler();

app.UseCors("AllowGateway");
app.MapGet("/hello", () => "Hello World!").AllowAnonymous();

//app.MapPost("/events/notification",
//    async (EventDto dto, HttpRequest request, INotificationMessageFactory messageFactory, INotificationCommand command) =>
//    {
//        try
//        {
//            var topic = request.Headers["ce-type"].FirstOrDefault();

//            var message = await messageFactory.BuildMessageAsync(topic, dto);

//            await command.CreateNotificationAsync(dto.UserId, message);

//            return Results.Created();
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine(ex.Message);
//            return Results.Problem(ex.Message);
//        }
//    })
//    //.WithTopic("pubsub", "post-published")
//    .WithTopic("pubsub", "comment-published")
//    .WithTopic("pubsub", "post-rejected")
//    .WithTopic("pubsub", "comment-rejected")
//    .WithTopic("pubsub", "post-upvote-created")
//    .WithTopic("pubsub", "post-downvote-created")
//    .WithTopic("pubsub", "comment-upvote-created")
//    .WithTopic("pubsub", "comment-downvote-created");



app.MapGet("/{userId}/notifications",
    async (string userId, INotificationQuery query) =>
    {
        try
        {
            var notifications = await query.GetNotificationsForUserAsync(userId);
            return Results.Ok(notifications);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return Results.Problem(ex.Message);
        }
    });

//app.MapPost("/events/post-published",
//    async (PostPublishedEventDto dto, IEventHandler eventHandler) =>
//    {
//        try
//        {
//            await eventHandler.ForumSubscribersRequested(dto.ForumId, dto.PostId);

//            return Results.Accepted();
//        }
//        catch (Exception ex)
//        {
//            return Results.Problem(ex.Message);
//        }
//    }).WithTopic("pubsub", "post-published");

app.MapPost("/events/post-published", // Only for testing
    async (PostPublishedEventDtoTest postDto, INotificationMessageFactory messageFactory, INotificationCommand command) =>
    {
        try
        {
            var topic = "post-published";

            var eventDto = new EventDto(postDto.UserId, postDto.ForumId, postDto.PostId, 0);

            var message = await messageFactory.BuildMessageAsync(topic, eventDto);

            await command.CreateNotificationAsync(eventDto.UserId, message);

            return Results.Created();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }

    }).WithTopic("pubsub", "post-published");

app.MapPost("/events/requested-forum-subscribers-collected",
    async (RequestedForumSubscribersCollectedEventDto dto, INotificationMessageFactory messageFactory, INotificationCommand command) =>
    {
        var message = messageFactory.BuildForPostPublished(dto);

        var tasks = dto.UserIds.Select(userId => command.CreateNotificationAsync(userId, message));
        await Task.WhenAll(tasks);

    }).WithTopic("pubsub", "requested-forum-subscribers-collected");

//app.MapPost("events/comment-published",
//    async (CommentPublishedEventDto dto, INotificationMessageFactory messageFactory, INotificationCommand command) =>
//    {
//        try
//        {
//            var message = messageFactory.Build(dto);

//            await command.CreateNotificationAsync(dto.UserId, message);

//            return Results.Created();
//        }
//        catch (Exception ex)
//        {
//            return Results.Problem(ex.Message);
//        }
//    }).WithTopic("pubsub", "comment-published");

app.Run();


