using ContentSafetyService.Domain;
using ContentSafetyService.Domain.Enums;

namespace ContentSafetyService.Application.Services.ProxyInterface;

public interface IContentSafetyDetection
{
    Task<DetectionResult> ContentDetectionAsync(MediaType mediaType, string content, string[] blocklists);
}