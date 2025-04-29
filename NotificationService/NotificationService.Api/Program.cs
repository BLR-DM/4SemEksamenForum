using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;
using NotificationService.Application;
using NotificationService.Application.Commands.CommandDto;
using NotificationService.Application.Commands.Interfaces;
using NotificationService.Application.Queries;
using NotificationService.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.BackchannelHttpHandler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        };

        options.Authority = "https://141.147.31.37:8443/realms/4SemForumProjekt";
        options.Audience = "notification-api";
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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

//app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.UseCloudEvents();


app.MapGet("/hello", () => "Hello World!").AllowAnonymous();

app.MapPost("/notification",
    async (CreateNotificationDto notificationDto, INotificationCommand command) =>
    {
        try
        {
            await command.CreateNotificationAsync(notificationDto.userId, notificationDto.Message);

            return Results.Created();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return Results.Problem(ex.Message);
        }
    });

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


app.Run();


