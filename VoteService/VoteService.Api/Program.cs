using System.Security.Claims;
using Dapr.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;
using VoteService.Application;
using VoteService.Application.Commands.CommandDto;
using VoteService.Application.Interfaces;
using VoteService.Application.Queries.Interfaces;
using VoteService.Infrastructure;

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

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();
//KEYCLOAK OPSÆTNING
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "https://keycloak.blrforum.dk/realms/4SemForumProjekt";
        options.Audience = "voteservice-api";
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

    app.MapScalarApiReference();

    
}

app.UseSwagger();
app.UseSwaggerUI();

//app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseCloudEvents();


app.MapPost("/api/Post/{postId}/Vote",
    async (int postId, PostVoteDto dto, IPostVoteCommand command, ClaimsPrincipal user) =>
    {
        try
        {
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            await command.TogglePostVote(postId, dto, userId);
            return Results.Ok();
        }
        catch (Exception e)
        {
            return Results.Problem(e.Message);
        }
    }).RequireAuthorization("StandardUser");

app.MapPost("/api/Comment/{commentId}/Vote",
    async (int commentId, CommentVoteDto dto, ICommentVoteCommand command, ClaimsPrincipal user) =>
    {
        try
        {
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
            await command.ToggleCommentVote(commentId, dto, userId);
            return Results.Ok();
        }
        catch (Exception e)
        {
            return Results.Problem(e.Message);
        }
    }).RequireAuthorization("StandardUser");

app.MapGet("/api/Post/{postId}/Votes",
    async (int postId, IPostVoteQuery query) =>
    {
        try
        {
            var postVotes = await query.GetVotesByPostIdAsync(postId);

            return Results.Ok(postVotes);
        }
        catch (Exception e)
        {
            return Results.Problem(e.Message);
        }
    }).RequireAuthorization("StandardUser");

app.MapPost("/api/Post/Votes",
    async ([FromBody] List<int> postIds, IPostVoteQuery query) =>
    {
        try
        {
            var postVotesList = await query.GetVotesByPostIdsAsync(postIds);

            return Results.Ok(postVotesList);
        }
        catch (Exception e)
        {
            return Results.Problem(e.Message);
        }
    }).RequireAuthorization("StandardUser");

app.MapGet("/api/Comment/{commentId}/Votes",
    async (int commentId, ICommentVoteQuery query) =>
    {
        try
        {
            var commentVores = await query.GetVotesByCommentIdAsync(commentId);

            return Results.Ok(commentVores);
        }
        catch (Exception e)
        {
            return Results.Problem(e.Message);
        }
    }).RequireAuthorization("StandardUser");

app.MapPost("/api/Comment/Votes",
    async ([FromBody] List<int> commentIds, ICommentVoteQuery query) =>
    {
        try
        {
            var commentVotesList = await query.GetVotesByCommentIdsAsync(commentIds);

            return Results.Ok(commentVotesList);
        }
        catch (Exception e)
        {
            return Results.Problem(e.Message);
        }
    }).RequireAuthorization("StandardUser");

app.MapGet("/api/hello", () => "Hello World!").RequireAuthorization("StandardUser");


app.Run();
