using ContentSafetyService.Application;
using ContentSafetyService.Application.Commands;
using ContentSafetyService.Domain.Enums;
using ContentSafetyService.Infrastructure;
using Dapr;
using Dapr.AppCallback.Autogen.Grpc.v1;
using Dapr.Client;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;
using Action = ContentSafetyService.Domain.Enums.Action;

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


app.UseRouting();
app.UseCloudEvents();
app.MapSubscribeHandler();

app.MapGet("/hello", () => "Hello World!");

app.MapPost("/events/contentmoderation",
    async (ILogger<Program> logger, ContentModerationDto payload, IContentSafetyCommand command, DaprClient dapr) =>
    {
        try
        {
            // Save moderation content request?
            logger.LogInformation("Content for moderation received:\n" +
                                  "ContentId: {Id}\n" +
                                  "Content: {Content}", payload.ContentId, payload.Content);

            var mediaType = MediaType.Text;
            var blocklists = Array.Empty<string>();

            var decision = await command.ModerateContentAsync(mediaType, payload.Content, blocklists);

            logger.LogInformation("Decision made by AI: {Decision}", decision.SuggestedAction);

            // Test

            var contentModeratedDto = new ContentModeratedDto(payload.ContentId, decision.SuggestedAction);
            await dapr.PublishEventAsync("pubsub", "content-moderated", contentModeratedDto);

            return Results.Ok();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return Results.Problem(ex.Message);
        }

    })
    .WithTopic("pubsub", "forum-submitted")
    .WithTopic("pubsub", "post-submitted")
    .WithTopic("pubsub", "comment-submitted");

app.Run();
public record ContentModerationDto(string ContentId, string Content);
public record ContentModeratedDto(string ContentId, Action Result);