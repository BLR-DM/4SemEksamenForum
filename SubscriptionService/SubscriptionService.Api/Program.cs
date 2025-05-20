using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;
using SubscriptionService.Api.Endpoints;
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

//KEYCLOAK OPSÆTNING
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "https://keycloak.blrforum.dk/realms/4SemForumProjekt";
        options.Audience = "subscriptionservice-api";
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Moderator", policy =>
    {
        policy.RequireRole("moderator");
    });
    options.AddPolicy("StandardUser", policy =>
    {
        policy.RequireRole("standard-user");
    });
});

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddApplication();

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


app.MapGet("/api/hello", () => "Hello World!").RequireAuthorization("StandardUser");


app.MapPost("/api/Forums/{forumId}/Subscriptions",
    async (int forumId, CreateSubDto dto, IForumSubCommand command, ClaimsPrincipal user) =>
    {
        try
        {
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            await command.CreateAsync(dto.ForumId, userId);

            return Results.Ok();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return Results.Problem(ex.Message);
        }
    }).RequireAuthorization("StandardUser");


app.MapDelete("/api/Forums/{forumId}/Subscriptions",
    async (int forumId, IForumSubCommand command, ClaimsPrincipal user) =>
    {
        try
        {
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            await command.DeleteAsync(forumId, userId);

            return Results.Ok();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return Results.Problem(ex.Message);
        }
    }).RequireAuthorization("StandardUser");

app.MapGet("/api/Forum/{forumId}/Subscriptions", async (int forumId, IForumSubQuery query) =>
{
    try
    {
        var appUserIds = await query.GetAppUserIdForSubscriptionsByForumIdAsync(forumId);

        return Results.Ok(appUserIds);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
        return Results.Problem(ex.Message);
    }
}).RequireAuthorization("StandardUser");

app.MapGet("/api/Users/{appUserId}/Forums/Subscriptions", async (string appUserId, IForumSubQuery query) =>
{
    try
    {
        var forumIds = await query.GetForumIdForSubscriptionsByUserIdAsync(appUserId);

        return Results.Ok(forumIds);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
        return Results.Problem(ex.Message);
    }
}).RequireAuthorization("StandardUser");

app.MapGet("/api/Post/{postId}/Subscriptions", async (int postId, IPostSubQuery query) =>
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
}).RequireAuthorization("StandardUser");

app.MapGet("/api/Users/{appUserId}/Posts/Subscriptions", async (string appUserId, IPostSubQuery query) =>
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
}).RequireAuthorization("StandardUser");

app.MapEventEndpoints();

app.Run();

public record ForumPublishedDto(string UserId, int ForumId);