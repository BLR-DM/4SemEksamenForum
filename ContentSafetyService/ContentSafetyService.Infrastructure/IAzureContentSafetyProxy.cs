using ContentSafetyService.Domain;
using ContentSafetyService.Domain.Enums;

namespace ContentSafetyService.Infrastructure;

public interface IAzureContentSafetyProxy
{
    Task<DetectionResult> DetectAsync(HttpRequestMessage msg, MediaType mediaType);
}