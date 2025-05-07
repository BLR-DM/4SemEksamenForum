using Gateway.Api.Dtos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddHttpClient();

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

//KEYCLOAK OPSÆTNING
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.BackchannelHttpHandler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        };

        options.Authority = "https://keycloak.blrforum.dk//realms/4SemForumProjekt";
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

builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowWebService",
        policy =>
        {
            policy.WithOrigins("http://localhost:5221") // Din Blazor-klients adresse
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

//app.UseHttpsRedirection();

app.UseCors("AllowWebService");

app.MapGet("/api/Forums/{forumName}/posts", async (string forumName, HttpClient httpClient) =>
{
    // Get Forum with Posts and Comments
    var forumRequestUri = $"http://contentservice-api:8080/forum/{forumName}/posts";
    var forum = await httpClient.GetFromJsonAsync<ForumDto>(forumRequestUri);

    if (forum == null) return Results.NotFound("Forum not found");
    
    // Get PostVotes for each post in forum
    var postVoteList = new List<PostVoteListDto>();

    var postVotesRequestUri = $"http://voteservice-api:8080/post/votes";
    var postIds = forum.Posts.Select(p => p.Id).ToList();

    var response = await httpClient.PostAsJsonAsync(postVotesRequestUri, postIds);

    if (!response.IsSuccessStatusCode) return Results.BadRequest("Failed to get PostVotes");

    postVoteList = await response.Content.ReadFromJsonAsync<List<PostVoteListDto>>();


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

});



app.UseAuthentication();
app.UseAuthorization();
app.MapReverseProxy();

app.Run();

