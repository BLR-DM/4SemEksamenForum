using System.Security.Claims;
using Dapr;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;
using PointService.Api.Endpoints;
using PointService.Application;
using PointService.Application.Command;
using PointService.Application.Command.CommandDto;
using PointService.Application.EventDto.PosVoteDto;
using PointService.Application.Queries;
using PointService.Infrastructure;
using PointService.Infrastructure.Queries;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDaprClient();

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

//KEYCLOAK OPSÆTNING  
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.BackchannelHttpHandler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        };

        options.Authority = "https://keycloak.blrforum.dk/realms/4SemForumProjekt";
        options.Audience = "pointservice-api";
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
app.MapGet("/api/hello", () => "Hello Api!");

app.MapPost("/api/User/{userId}/Points",
    async (string userId, CreatePointEntryDto dto, IPointEntryCommand command) =>
    {
        try
        {
            await command.CreatePointEntryAsync(dto, userId);

            return Results.Created();
        }
        catch (Exception)
        {
            return Results.Problem();
        }
    });

app.MapPost("/api/PointAction",
    async (CreatePointActionDto dto, IPointActionCommand command) =>
    {
        try
        {
            await command.CreatePointActionAsync(dto);

            return Results.Created();
        }
        catch (Exception)
        {
            return Results.Problem();
        }
    });

app.MapGet("/api/pointactions",
    async (IPointActionQuery query) =>
    {
        try
        {
            var pointActions = await query.GetAllPointActionsAsync();
            return Results.Ok(pointActions);
        }
        catch (Exception)
        {
            return Results.Problem();
        }
    });

app.MapGet("/api/User/{userId}/Points",
    async (string userId, ClaimsPrincipal user, IPointEntryQuery query) =>
    {
        try
        {
            var appUserId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;


            if (appUserId is null)
                return Results.Problem();
            
            var points = await query.GetPointsByUserIdAsync(appUserId);

            return Results.Ok(points);
        }
        catch (Exception)
        {
            return Results.Problem();
        }
    }).RequireAuthorization();

app.MapEventEndpoints();

app.MapPut("/api/PointAction",
    async (UpdatePointActionDto dto, IPointActionCommand command) =>
    {
        try
        {
            await command.UpdatePointActionAsync(dto);
            return Results.Ok();
        }
        catch (Exception)
        {
            return Results.Problem();
        }
    });


app.Run();

