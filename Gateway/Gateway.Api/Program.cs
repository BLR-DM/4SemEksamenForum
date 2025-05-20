using Gateway.Api.Dtos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container. 
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddHttpClient();

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;

    // Adjust if needed – this is usually the Docker gateway IP for bridge networks
    options.KnownProxies.Add(IPAddress.Parse("172.18.0.1"));
});

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

//KEYCLOAK OPSÆTNING
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "https://keycloak.blrforum.dk/realms/4SemForumProjekt";
        options.Audience = "gateway-api";
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
    options.AddPolicy(name: "AllowWebService",
        policy =>
        {
            policy.WithOrigins(
                    "https://blrforum.dk",       // Production
                    "https://www.blrforum.dk",   // Alternate production
                    "http://localhost:5221"      // Development
                )
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();  // Required for cookies/auth headers
        });
});

var app = builder.Build();

app.UseForwardedHeaders();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

//app.UseHttpsRedirection();

app.UseCors("AllowWebService");


app.MapGet("/api/Forums/{forumName}/posts", async (string forumName, HttpClient httpClient, HttpContext context) =>
{

    if (context.Request.Headers.TryGetValue("Authorization", out var authHeader))
    {
        var authHeaderValue = authHeader.ToString();
        var token = authHeaderValue.Substring("Bearer ".Length);
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
    }
    else
    {
        return Results.Unauthorized();
    }


        // Get Forum with Posts and Comments
        var forumRequestUri = $"http://contentservice-api:8080/api/forum/{forumName}/posts";
    var forum = await httpClient.GetFromJsonAsync<ForumDto>(forumRequestUri);

    if (forum == null) return Results.NotFound("Forum not found");
    
    // Get PostVotes for each post in forum
    //var postVoteList = new List<PostVoteListDto>();

    var postVotesRequestUri = $"http://voteservice-api:8080/api/post/votes";
    var postIds = forum.Posts?.Select(p => p.Id).ToList();

    if (postIds == null || postIds.Count == 0)
        return Results.Ok(forum);

    var response = await httpClient.PostAsJsonAsync(postVotesRequestUri, postIds);

    if (!response.IsSuccessStatusCode) return Results.BadRequest("Failed to get PostVotes");

    var postVoteList = await response.Content.ReadFromJsonAsync<List<PostVoteListDto>>();


    // Map PostVotes to Posts
    if (postVoteList != null)
    {
        foreach (var post in forum.Posts)
        {
            post.Votes = postVoteList.FirstOrDefault(pv => pv.PostId == post.Id)?.PostVotes ?? null;

        }
    }
    
    //GET VOTES FOR COMMENT

    return Results.Ok(forum);

}).RequireAuthorization("StandardUser");

app.MapGet("/api/forums/{forumName}/posts/{postId}", async (string forumName, int postId, HttpClient httpClient, HttpContext context) =>
{
    if (context.Request.Headers.TryGetValue("Authorization", out var authHeader))
    {
        var authHeaderValue = authHeader.ToString();
        var token = authHeaderValue.Substring("Bearer ".Length);
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
    }
    else
    {
        return Results.Unauthorized();
    }

    // Get Forum with single Post and Comments
    var forumRequestUri = $"http://contentservice-api:8080/api/forums/{forumName}/post/{postId}";
    var forum = await httpClient.GetFromJsonAsync<ForumDto>(forumRequestUri);

    if (forum == null) return Results.NotFound("Forum not found");

    // Get Votes for post
    var postVotesRequestUri = $"http://voteservice-api:8080/api/Post/{postId}/Votes";
    var postVotes = await httpClient.GetFromJsonAsync<PostVoteListDto>(postVotesRequestUri);

    if (postVotes == null)
        return Results.BadRequest("Failed to get PostVotes");

    var post = forum.Posts.FirstOrDefault(p => p.Id == postId);

    if (post == null) return Results.Ok(forum);

    post.Votes = postVotes.PostVotes;

    // Get votes for comments
    var commentVotesRequestUri = $"http://voteservice-api:8080/api/Comment/Votes";
    var commentIds = post.Comments?.Select(c => c.Id).ToList();

    if (commentIds == null || commentIds.Count == 0)
        return Results.Ok(forum);

    var commentVotesResponse = await httpClient.PostAsJsonAsync(commentVotesRequestUri, commentIds);

    if (!commentVotesResponse.IsSuccessStatusCode) return Results.BadRequest("Failed to get CommentVotes");

    var commentVotesList = await commentVotesResponse.Content.ReadFromJsonAsync<List<CommentVoteListDto>>();

    // Map CommentVotes to Comments
    if (commentVotesList != null && post.Comments != null)
    {
        foreach (var comment in post.Comments)
        {
            comment.Votes = commentVotesList.FirstOrDefault(cv => cv.CommentId == comment.Id)?.CommentVotes ?? null;
        }
    }

    return Results.Ok(forum);
}).RequireAuthorization("StandardUser");



app.UseAuthentication();
app.UseAuthorization();
app.MapReverseProxy();

app.Run();

