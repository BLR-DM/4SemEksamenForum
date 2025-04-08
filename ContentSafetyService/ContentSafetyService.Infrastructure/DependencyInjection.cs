using ContentSafetyService.Application.Configuration;
using ContentSafetyService.Application.Services;
using ContentSafetyService.Application.Services.ProxyInterface;
using ContentSafetyService.Infrastructure.Configuration;
using ContentSafetyService.Infrastructure.Helpers;
using ContentSafetyService.Infrastructure.ServiceProxyImpl;
using ContentSafetyService.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ContentSafetyService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ContentSafetySettings>(configuration.GetSection("ContentSafetySettings"));
        services.Configure<RejectionThresholds>(configuration.GetSection("RejectionThresholds"));

        services.AddScoped<IRequestBuilder, RequestBuilder>();
        services.AddScoped<IRejectionThresholdProvider, RejectionThresholdProvider>();
        services.AddScoped<IAzureContentSafetyProxy, AzureContentSafetyProxy>();
        services.AddScoped<IContentSafetyDetection, ContentSafetyDetection>();
        return services;
    }
}