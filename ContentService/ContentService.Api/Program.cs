using ContentService.Api.Endpoints;
using ContentService.Application;
using ContentService.Application.Commands.CommandDto.ForumDto;
using ContentService.Application.Commands.Interfaces;
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
//builder.Services.AddSwaggerGen(options =>
//{
//    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
//    {
//        In = ParameterLocation.Header,
//        Description = "Please enter your token as 'Bearer {your_token}'",
//        Name = "Authorization",
//        Type = SecuritySchemeType.ApiKey
//    });

//    options.AddSecurityRequirement(new OpenApiSecurityRequirement
//    {
//        {
//            new OpenApiSecurityScheme
//            {
//                Reference = new OpenApiReference
//                {
//                    Type = ReferenceType.SecurityScheme,
//                    Id = "Bearer"
//                }
//            },
//            new string[] { }
//        }
//    });
//});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

//builder.Services.AddAuthorization(options =>
//{
//    options.FallbackPolicy = new AuthorizationPolicyBuilder()
//        .RequireAuthenticatedUser()
//        .Build();
//});

//builder.Services.AddAuthorization();

//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
//    opt =>
//    {
//        opt.BackchannelHttpHandler = new HttpClientHandler
//        {
//            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
//        };
//        opt.RequireHttpsMetadata = false;
//        opt.MetadataAddress = builder.Configuration["Keycloak:MetadataAddress"]!;
//        opt.TokenValidationParameters = new TokenValidationParameters
//        {
//            ValidAudience = builder.Configuration["Keycloak:Audience"],
//            ValidIssuer = builder.Configuration["Keycloak:ValidIssuer"],
//            ValidateIssuer = true,
//            ValidateAudience = true
//        };
//        opt.Events = new JwtBearerEvents
//        {
//            OnAuthenticationFailed = ctx =>
//            {
//                Console.WriteLine($"JWT Auth failed: {ctx.Exception}");
//                return Task.CompletedTask;
//            },
//            OnTokenValidated = ctx =>
//            {
//                Console.WriteLine("JWT Auth success!");
//                return Task.CompletedTask;
//            }
//        };
//    });



builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAspire", builder =>
    {
        builder.WithOrigins("https://localhost:7293", "http://localhost:5012")
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

//KEYCLOAK OPSÆTNING
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.BackchannelHttpHandler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        };

        options.Authority = "https://141.147.31.37:8443/realms/4SemForumProjekt";
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

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseCors("AllowAspire");


// Enable DI validation at startup
//builder.Services.AddOptions<ServiceProviderOptions>()
//    .Configure(options => options.ValidateOnBuild = true);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.MapScalarApiReference();
    
}


//app.UseSwagger();
//app.UseSwaggerUI();

//app.UseHttpsRedirection(); 


app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseCloudEvents();
app.MapSubscribeHandler();

app.MapGet("/hello", () => "Hello World!").RequireAuthorization();

//app.MapPost("/content-moderated",
//    async (IForumCommand command, ContentApprovedDto contentId) =>
//    {
//        Console.WriteLine(contentId.ContentId);
//        var contentType = contentId.ContentId.Split(':')[0]; // Forum
//        // If forum
//        var publishForumDto = new PublishForumDto(14);
//        await command.HandleForumApprovalAsync(publishForumDto);
//        return Results.Ok();
//    })
//    .WithTopic("pubsub", "content-moderated")
//    .AllowAnonymous();


var eventGroup = app.MapGroup("/events"); //.RequireAuthorization("Internal")

eventGroup.MapPost("/content-moderated",
    async (IModerationResultHandler handler, ContentModeratedDto dto) =>
    {
        Console.WriteLine($"Received moderation result: {dto.ContentId} = {dto.Result}");
        await handler.HandleModerationResultAsync(dto);
        return Results.Ok();
    })
    .WithTopic("pubsub", "content-moderated")
    .AllowAnonymous();

// Event
eventGroup.MapPost("/compensate-delete-forum",
    async (IForumCommand command, FailedToSubscribeUserToForumEventDto evt) =>
    {
        await command.DeleteForumAsync(evt.AppUserId, evt.ForumId);
        return Results.NoContent();
    })
    .WithTopic("pubsub", "user-subscribed-to-forum-on-creation-failed")
    .AllowAnonymous();





//app.MapPost("/publish", async (DaprClient daprClient) =>
//{
//    await daprClient.PublishEventAsync("pubsub", "test-topic", new MessagePayload("Hello From ContentService API!"));
//    return Results.Ok();
//}).AllowAnonymous();


/* Flow:
ContentService => contentSubmitted => ContentSafetyService moderates => contentApproved => ContentService saves => contentToPublish => ContentService saves => contentPublished

With workflow:
// High-Level Overview //

1. User Creates Forum → ContentService receives request.

2. Triggers Workflow → WorkflowService orchestrates the process.

3. Runs Content Safety Checks → ContentSafetyService validates content.

4. Updates State Store → If approved, ContentService persists forum.

5. Workflow Completes → Final status is updated, and forum is published.

*/
app.MapForumEndpoints();
app.MapPostEndpoints();
app.MapCommentEndpoints();

app.Run();
public record MessagePayload(string Text);
