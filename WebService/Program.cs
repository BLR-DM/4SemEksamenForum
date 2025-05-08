using System.Collections.Immutable;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using WebService;
using WebService.Proxies;
using WebService.Services;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient("GatewayApi",
        client => client.BaseAddress = new Uri("http://localhost:5000"))
    .AddHttpMessageHandler(sp =>
    {
        var handler = sp.GetRequiredService<AuthorizationMessageHandler>()
            .ConfigureHandler(
                authorizedUrls: ["http://localhost:5000"]);

        return handler;
    });

//builder.Services.AddHttpClient("GatewayApi",
//        client => client.BaseAddress = new Uri("http://localhost:5000"));

builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("GatewayApi"));

builder.Services.AddScoped<IApiProxy, ApiProxy>();

builder.Services.AddScoped<IForumService, ForumService>();

builder.Services.AddScoped<IContentServiceProxy, ContentServiceProxy>();

builder.Services.AddScoped<ISubscriptionServiceProxy, SubscriptionServiceProxy>();

builder.Services.AddScoped<ISubscriptionService, SubscriptionService>();

builder.Services.AddScoped<UserSessionService, UserSessionService>();

builder.Services.AddScoped<IPostService, PostService>();

builder.Services.AddScoped<IVoteService, VoteService>();

builder.Services.AddScoped<IVoteServiceProxy, VoteServiceProxy>();

builder.Services.AddBlazoredSessionStorage();


builder.Services.AddOidcAuthentication(options =>
{
    options.ProviderOptions.Authority = "https://keycloak.blrforum.dk/realms/4SemForumProjekt";
    options.ProviderOptions.ClientId = "webservice-client";
    options.ProviderOptions.ResponseType = "code";
    options.ProviderOptions.DefaultScopes.Add("openid");
    options.ProviderOptions.DefaultScopes.Add("profile");
    options.ProviderOptions.DefaultScopes.Add("email");
    options.ProviderOptions.DefaultScopes.Add("webservice_api_scope");
});



builder.Services.AddMudServices();

await builder.Build().RunAsync();
