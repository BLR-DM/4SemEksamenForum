using ContentService.Api.Endpoints;
using ContentService.Application;
using ContentService.Application.Commands.CommandDto.ForumDto;
using ContentService.Application.Commands.Interfaces;
using ContentService.Application.EventDto;
using ContentService.Application.Services;
using ContentService.Infrastructure;
using ContentService.Infrastructure.Interfaces;
using Dapr;
using Dapr.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDaprClient();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);


//KEYCLOAK OPSÆTNING
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "https://keycloak.blrforum.dk/realms/4SemForumProjekt";
        options.Audience = "contentservice-api";
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


// Enable DI validation at startup
//builder.Services.AddOptions<ServiceProviderOptions>()
//    .Configure(options => options.ValidateOnBuild = true);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.MapScalarApiReference();
    
}

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseCloudEvents();
app.MapSubscribeHandler();

app.MapGet("/api/hello", () => "Hello World!").RequireAuthorization("StandardUser");


app.MapForumEndpoints();
app.MapPostEndpoints();
app.MapCommentEndpoints();
app.MapEventEndpoints();

app.Run();
