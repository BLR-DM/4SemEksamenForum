using ContentSafetyService.Domain.ValueObjects;

namespace ContentSafetyService.Application.CommandDto;

public record PostDtoTest()
{
    public int PostId { get; set; }
    public DateTime Date { get; set; }
    public Status Status { get; set; }
    public Content Content { get; set; }
}

public enum Status
{
    Submitted,
    Pending,
    Review,
    Approved,
    Published,
    Rejected
}