using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;
using SubscriptionService.Application.Commands;
using SubscriptionService.Application.Commands.CommandDto;
using SubscriptionService.Application.Commands.Interfaces;
using SubscriptionService.Application.Configuration;
using SubscriptionService.Application.Dto;
using SubscriptionService.Application.EventDto;
using SubscriptionService.Application.Queries.Interfaces;
using SubscriptionService.Application.Services;
using SubscriptionService.Infrastructure.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDaprClient();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter your token as 'Bearer {your_token}'",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

//KEYCLOAK OPS�TNING
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.BackchannelHttpHandler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        };

        options.Authority = "https://keycloak.blrforum.dk/realms/4SemForumProjekt";
        options.Audience = "subscription-api";
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

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddApplication();

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

    app.MapScalarApiReference();
    
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();
app.UseCloudEvents();
app.MapSubscribeHandler();
//app.UseHttpsRedirection();

app.UseCors("AllowGateway");

app.MapGet("/hello", () => "Hello World!").RequireAuthorization();


app.MapPost("/Forums/{forumId}/Subscriptions",
    async (int forumId, [FromBody] CreateSubDto subDto, IForumSubCommand command) =>
    {
        try
        {
            await command.CreateAsync(forumId, subDto.AppUserId);

            return Results.Ok();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return Results.Problem(ex.Message);
        }
    });


app.MapPost("/events/forum-published",
    async (ForumPublishedDto evtDto, IForumSubCommand command) =>
    {
        try
        {
            await command.CreateAsync(evtDto.ForumId, evtDto.UserId);

            return Results.Ok();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return Results.Problem(ex.Message);
        }
    }).WithTopic("pubsub", "forum-published");


app.MapPost("/Posts/{postId}/Subscriptions",
    async (int postId, [FromBody] CreateSubDto subDto, IPostSubCommand command) =>
    {
        try
        {
            await command.CreateAsync(postId, subDto.AppUserId);
            return Results.Ok();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return Results.Problem(ex.Message);
        }
    });

app.MapPost("/events/post-published",
    async (PostPublishedDto evtDto, IForumSubCommand forumSubCommand, IPostSubCommand postSubCommand) =>
    {
        try
        {
            await forumSubCommand.CreateAsync(evtDto.ForumId, evtDto.UserId);
            await postSubCommand.CreateAsync(evtDto.PostId, evtDto.UserId);

            return Results.Ok();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return Results.Problem(ex.Message);
        }
    }).WithTopic("pubsub", "post-published");


app.MapPost("/events/forum-subscribers-requested",
    async (ForumSubscribersRequestedEventDto evtDto, IForumSubQuery query, IEventHandler eventHandler) =>
    {
        var userIds = await query.GetSubscriptionsByForumIdAsync(evtDto.ForumId);
        await eventHandler.RequestedForumSubscribersCollected(userIds, evtDto.ForumId, evtDto.PostId);

    }).WithTopic("pubsub", "forum-subscribers-requested");

//app.MapPost("/events/notify-forum-subscribers",
//    async (NotifyForumSubscriberEventDto evtDto, IPostPublishedHandler handler) =>
//    {
//        try
//        {
//            await handler.HandlePostPublishedNotification(evtDto);
//            return Results.Ok();
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine(ex.Message);
//            return Results.Problem(ex.Message);
//        }
//    }).WithTopic("pubsub", "forum-notification-requested");


//app.MapPost("/events/comment-published",
//    async (CommentPublishedDto evtDto, ICommentPublishedHandler handler) =>
//    {
//        try
//        {
//            await handler.HandleCommentPublished(evtDto);
//            return Results.Ok();
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine(ex.Message);
//            return Results.Problem(ex.Message);
//        }
//    }).WithTopic("pubsub", "comment-published");

//app.MapPost("/events/notify-post-subscribers",
//    async (NotifyPostSubscriberEventDto evtDto, ICommentPublishedHandler handler) =>
//    {
//        try
//        {
//            await handler.HandleCommentPublishedNotification(evtDto);
//            return Results.Ok();
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine(ex.Message);
//            return Results.Problem(ex.Message);
//        }
//    }).WithTopic("pubsub", "post-notification-requested");

app.MapDelete("/Forums/{forumId}/Subscriptions",
    async (int forumId, [FromBody] CreateSubDto subDto, IForumSubCommand command) =>
    {
        try
        {
            await command.DeleteAsync(forumId, subDto.AppUserId);

            return Results.Ok();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return Results.Problem(ex.Message);
        }
    });

app.MapDelete("/Posts/{postId}/Subscriptions",
    async (int postId, [FromBody] CreateSubDto subDto, IPostSubCommand command) =>
    {
        try
        {
            await command.DeleteAsync(postId, subDto.AppUserId);
            return Results.Ok();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return Results.Problem(ex.Message);
        }
    });

app.MapGet("/Forum/{forumId}/Subscriptions/", async (int forumId, IForumSubQuery query) =>
{
    try
    {
        var appUserIds = await query.GetSubscriptionsByForumIdAsync(forumId);

        return Results.Ok(appUserIds);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
        return Results.Problem(ex.Message);
    }
});

app.MapGet("/Users/{appUserId}/Forums/Subscriptions/", async (string appUserId, IForumSubQuery query) =>
{
    try
    {
        var forumIds = await query.GetSubscriptionsByUserIdAsync(appUserId);

        return Results.Ok(forumIds);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
        return Results.Problem(ex.Message);
    }
});

app.MapGet("/Post/{postId}/Subscriptions/", async (int postId, IPostSubQuery query) =>
{
    try
    {
        var appUserIds = await query.GetSubscriptionsByPostIdAsync(postId);
        return Results.Ok(appUserIds);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
        return Results.Problem(ex.Message);
    }
});

app.MapGet("/Users/{appUserId}/Posts/Subscriptions/", async (string appUserId, IPostSubQuery query) =>
{
    try
    {
        var test = await query.GetSubscriptionsByUserIdAsync(appUserId);

        var response = ResponseDto<List<int>>.Ok(test);

        return Results.Ok(response);
    }
    catch (Exception ex)
    {
        var response = ResponseDto<List<int>>.Fail(ex.Message);
        return Results.Ok(response);
    }
});


app.Run();

public record ForumPublishedDto(string UserId, int ForumId);