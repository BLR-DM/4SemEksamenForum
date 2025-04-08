using ContentSafetyService.Domain;
using ContentSafetyService.Domain.Enums;

namespace ContentSafetyService.Application.Commands;

public interface IContentSafetyCommand
{
    Task<Decision> ModerateContentAsync(MediaType mediaType, string content, string[] blocklists);
}