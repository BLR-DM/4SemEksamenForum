using ContentSafetyService.Application.Services.ProxyInterface;
using ContentSafetyService.Domain;
using ContentSafetyService.Domain.Enums;

namespace ContentSafetyService.Infrastructure.Services;

public class ContentSafetyDetection : IContentSafetyDetection
{
    private readonly IAzureContentSafetyProxy _azureContentSafetyProxy;
    private readonly IRequestBuilder _requestBuilder;

    public ContentSafetyDetection(IAzureContentSafetyProxy azureContentSafetyProxy, IRequestBuilder requestBuilder)
    {
        _azureContentSafetyProxy = azureContentSafetyProxy;
        _requestBuilder = requestBuilder;
    }

    async Task<DetectionResult> IContentSafetyDetection.ContentDetectionAsync(MediaType mediaType, string content, string[] blocklists)
    {
        var message = _requestBuilder.BuildHttpRequestMessage(mediaType, content, blocklists);
        var result = await _azureContentSafetyProxy.DetectAsync(message, mediaType);

        return result;
    }
}