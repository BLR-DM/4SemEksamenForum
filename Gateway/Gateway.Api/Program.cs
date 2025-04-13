var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

//app.UseHttpsRedirection();

app.MapGet("/api/Forum/{forumId}/posts", (int forumId) => {
    return "hello";

    //get forum + posts from contentservice
    //get votes from voteservice
});

//app.MapGet("/api/Forum/{forumId}/posts", (int forumId) => {
//    return "hello";
//});

app.MapReverseProxy();

app.Run();

