using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using WebService;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient("GateWayApi",
        client => client.BaseAddress = new Uri("https://localhost:5000"))
    .AddHttpMessageHandler(sp =>
    {
        var handler = sp.GetRequiredService<AuthorizationMessageHandler>()
            .ConfigureHandler(
                authorizedUrls: ["https://localhost:5000"]);
    
        return handler;
    });

builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("GateWayApi"));


builder.Services.AddOidcAuthentication(options =>
{
    options.ProviderOptions.Authority = "https://141.147.31.37:8443/realms/4SemForumProjekt";
    options.ProviderOptions.ClientId = "webservice-client";
    options.ProviderOptions.ResponseType = "code";
    options.ProviderOptions.DefaultScopes.Add("webservice_api_scope");
    options.ProviderOptions.DefaultScopes.Add("openid");
    options.ProviderOptions.DefaultScopes.Add("profile");
    options.ProviderOptions.DefaultScopes.Add("email");
});


builder.Services.AddMudServices();

await builder.Build().RunAsync();
