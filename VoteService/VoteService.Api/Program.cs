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


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAspire", builder =>
    {
        builder.WithOrigins("https://localhost:7070")
            .WithOrigins("http://localhost:5048")
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

//app.UseHttpsRedirection();

app.UseCors("AllowAspire");


app.MapPost("Post/{postId}/Vote",
    async (string postId, PostVoteDto dto, IPostVoteCommand command) =>
    {
        try
        {
            await command.TogglePostVote(postId, dto);
            return Results.Ok();
        }
        catch (Exception e)
        {
            return Results.Problem(e.Message);
        }
    });

app.MapPost("Comment/{commentId}/Vote",
    async (string commentId, CommentVoteDto dto, ICommentVoteCommand command) =>
    {
        try
        {
            await command.ToggleCommentVote(commentId, dto);
            return Results.Ok();
        }
        catch (Exception e)
        {
            return Results.Problem(e.Message);
        }
    });

app.MapGet("Post/{postId}/Votes",
    async (string postId, IPostVoteQuery query) =>
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
    });

app.MapPost("Post/Votes",
    async (List<string> postIds, IPostVoteQuery query) =>
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
    });

app.MapGet("Comment/{commentId}/Votes",
    async (string commentId, ICommentVoteQuery query) =>
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
    });

app.MapPost("Comment/Votes",
    async (List<string> commentIds, ICommentVoteQuery query) =>
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
    });

app.MapGet("/hello", () => "Hello World!");

app.Run();
