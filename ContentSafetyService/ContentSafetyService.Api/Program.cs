using ContentSafetyService.Application;
using ContentSafetyService.Application.Commands;
using ContentSafetyService.Domain.Enums;
using ContentSafetyService.Domain.ValueObjects;
using ContentSafetyService.Infrastructure;
using Dapr;
using Dapr.AppCallback.Autogen.Grpc.v1;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

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

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAspire", builder =>
    {
        builder.WithOrigins("https://localhost:7214")
            .WithOrigins("http://localhost:5041")
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.AddHttpClient();

builder.Services.AddDaprClient();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// Enable DI validation at startup
builder.Services.AddOptions<ServiceProviderOptions>()
    .Configure(options => options.ValidateOnBuild = true);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.MapScalarApiReference();

    
}

app.UseSwagger();
app.UseSwaggerUI();


//app.UseHttpsRedirection(); 

app.UseCors("AllowAspire");

app.UseRouting();
app.UseCloudEvents();
app.MapSubscribeHandler();

app.MapGet("/hello", () => "Hello World!");




app.MapPost("/subscribe", (ILogger<Program> logger, MessagePayload text) =>
    {
        logger.LogInformation("Received message: {Text}", text.Text);
        return Results.Ok();
    })
    .WithTopic("pubsub", "test-topic");




app.MapPost("/contentmoderation",
    async (ILogger<Program> logger, ContentModerationDto payload, IContentSafetyCommand command) =>
    {
        // Save moderation content request?
        logger.LogInformation("Content for moderation received: Id: {Id}\n" +
                              "Content: {Content}", payload.Id, payload.Content);

        var mediaType = MediaType.Text;
        var blocklists = Array.Empty<string>();

        var decision = await command.ModerateContentAsync(mediaType, payload.Content, blocklists);

        logger.LogInformation("Decision made by AI: {Decision}", decision.SuggestedAction);

        return Results.Ok();
    }).WithTopic("pubsub", "contentSubmitted")
    .WithTopic("pubsub", "post-submitted");

app.Run();
public record MessagePayload(string Text);
public record ContentModerationDto(int Id, string Content);