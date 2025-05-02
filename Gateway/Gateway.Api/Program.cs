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

        options.Authority = "https://141.147.31.37:8443/realms/4SemForumProjekt";
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


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

//app.UseHttpsRedirection();

app.MapGet("/api/Forum/{forumName}/posts", async (string forumName, HttpClient httpClient) =>
{
    var forumRequestUri = $"http://contentservice-api:8080/forum/{forumName}/posts";
    var forum = await httpClient.GetFromJsonAsync<ForumDto>(forumRequestUri);

    var postIds = forum.Posts.Select(p => p.Id).ToList();

    //var votes = await client.InvokeMethodAsync<object, IEnumerable<GetPostVotesDto>>(
    //    HttpMethod.Post, "voteservice-dapr", "/posts/votes", voteRequest);

    var votesRequestUri = $"http://voteservice-api:8080/post/votes";
    var response = await httpClient.PostAsJsonAsync(votesRequestUri, postIds);

    if (response.IsSuccessStatusCode)
    {

    }

    var votes = await response.Content.ReadFromJsonAsync<IEnumerable<GetPostVotesDto>>();

    //var votes = await httpClient.GetFromJsonAsync<IEnumerable<GetPostVotesDto>>(votesRequestUri);

    foreach (var post in forum.Posts)
    {
        post.Votes = votes.FirstOrDefault(pv => pv.PostId == post.Id)?.PostVotes ?? null;

    }

    return Results.Ok(forum);

    //get forum + posts from contentservice
    //get votes from voteservice
});

//app.MapGet("/api/Forum/{forumId}/posts", (int forumId) => {
//    return "hello";
//});

app.UseAuthentication();
app.UseAuthorization();
app.MapReverseProxy();

app.Run();

