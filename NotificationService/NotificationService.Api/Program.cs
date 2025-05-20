using System.Security.Claims;
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
using NotificationService.Application.Queries;
using NotificationService.Application.Services;
using NotificationService.Infrastructure;
using System.Text.Json;
using NotificationService.Api.Endpoints;
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

app.MapGet("/api/hello", () => "Hello World!").RequireAuthorization("StandardUser");


app.MapGet("/api/{userId}/notifications",
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
    }).RequireAuthorization("StandardUser");


app.MapPatch("/api/notifications/{id}/read",
    async (int id, ClaimsPrincipal user, ISentNotificationCommand command) =>
    {
        try
        {
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            await command.MarkAsReadAsync(new MarkAsReadDto(userId, id));
            return Results.NoContent();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return Results.Problem(ex.Message);
        }
    }).RequireAuthorization("StandardUser");

app.MapEventEndpoints();

app.Run();


