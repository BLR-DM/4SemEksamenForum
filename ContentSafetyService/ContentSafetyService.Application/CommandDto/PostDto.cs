using ContentSafetyService.Domain.ValueObjects;

namespace ContentSafetyService.Application.CommandDto;

public record PostDto()
{
    public Content Content { get; set; } = new();
}