using ContentSafetyService.Domain;
using ContentSafetyService.Domain.Enums;

namespace ContentSafetyService.Application.Services;

public interface IDecisionService
{
    Decision MakeDecision(DetectionResult detectionResult, Dictionary<Category, int> rejectThresholds);
}