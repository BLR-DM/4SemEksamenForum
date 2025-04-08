using ContentSafetyService.Domain.Enums;

namespace ContentSafetyService.Application.Services;

public interface IRejectionThresholdProvider
{
    Dictionary<Category, int> GetRejectionThresholds();
}