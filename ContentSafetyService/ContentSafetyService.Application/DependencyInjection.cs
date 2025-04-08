using ContentSafetyService.Application.Commands;
using ContentSafetyService.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ContentSafetyService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IContentSafetyCommand, ContentSafetyCommand>();
        services.AddScoped<IDecisionService, DecisionService>();
        return services;
    }
}
