﻿using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using System.Collections.Immutable;
using WebService;
using WebService.Proxies;
using WebService.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var gatewayBaseUrl = builder.Configuration["GatewayBaseUrl"];
var authorizedUrls = builder.Configuration.GetSection("AuthorizedUrls").Get<string[]>();

builder.Services.AddHttpClient("GatewayApi",
        client => client.BaseAddress = new Uri(gatewayBaseUrl!))
    .AddHttpMessageHandler(sp =>
    {
        var handler = sp.GetRequiredService<AuthorizationMessageHandler>()
            .ConfigureHandler(authorizedUrls: authorizedUrls!);

        return handler;
    });

//builder.Services.AddHttpClient("GatewayApi", client =>
//    {
//        client.BaseAddress = new Uri("https://blrforum.dk/api/");
//    })
//    .AddHttpMessageHandler(sp =>
//    {
//        return sp.GetRequiredService<AuthorizationMessageHandler>()
//            .ConfigureHandler(
//                authorizedUrls: [
//                    "https://blrforum.dk/api", 
//                    "https://www.blrforum.dk/api"
//                ],
//                scopes: ["openid", "profile"]
//            );
//    });


//builder.Services.AddHttpClient("GatewayApi",
//        client => client.BaseAddress = new Uri("http://localhost:5000"));

builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("GatewayApi"));

builder.Services.AddScoped<IApiProxy, ApiProxy>();

builder.Services.AddScoped<IForumService, ForumService>();

builder.Services.AddScoped<IContentServiceProxy, ContentServiceProxy>();

builder.Services.AddScoped<ISubscriptionServiceProxy, SubscriptionServiceProxy>();

builder.Services.AddScoped<ISubscriptionService, SubscriptionService>();

builder.Services.AddScoped<UserSessionService>();

builder.Services.AddScoped<IPostService, PostService>();

builder.Services.AddScoped<IVoteService, VoteService>();

builder.Services.AddScoped<IVoteServiceProxy, VoteServiceProxy>();

builder.Services.AddScoped<IPointService, PointService>();

builder.Services.AddScoped<IPointServiceProxy, PointServiceProxy>();

builder.Services.AddScoped<INotificationService, NotificationService>();

builder.Services.AddScoped<INotificationServiceProxy, NotificationServiceProxy>();

builder.Services.AddBlazoredSessionStorage();


builder.Services.AddOidcAuthentication(options =>
{
    options.ProviderOptions.Authority = "https://keycloak.blrforum.dk/realms/4SemForumProjekt";
    options.ProviderOptions.MetadataUrl = "https://keycloak.blrforum.dk/realms/4SemForumProjekt/.well-known/openid-configuration";
    options.ProviderOptions.ClientId = "webservice-client";
    options.ProviderOptions.ResponseType = "code";

    options.ProviderOptions.DefaultScopes.Add("openid");
    options.ProviderOptions.DefaultScopes.Add("profile");
    options.ProviderOptions.DefaultScopes.Add("email");
    options.ProviderOptions.DefaultScopes.Add("webservice_api_scope");
});


builder.Services.AddMudServices();

await builder.Build().RunAsync();
