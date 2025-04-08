using ContentSafetyService.Domain.Enums;

namespace ContentSafetyService.Infrastructure.Services;

public interface IRequestBuilder
{
    HttpRequestMessage BuildHttpRequestMessage(MediaType mediaType, string content, string[] blocklists);
}