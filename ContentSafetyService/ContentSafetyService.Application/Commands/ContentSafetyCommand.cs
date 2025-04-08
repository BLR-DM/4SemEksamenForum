using ContentSafetyService.Application.Services;
using ContentSafetyService.Application.Services.ProxyInterface;
using ContentSafetyService.Domain;
using ContentSafetyService.Domain.Enums;

namespace ContentSafetyService.Application.Commands;

public class ContentSafetyCommand : IContentSafetyCommand
{
    private readonly IContentSafetyDetection _contentSafetyDetection;
    private readonly IDecisionService _decisionService;
    private readonly IRejectionThresholdProvider _rejectionThresholds;

    public ContentSafetyCommand(
        IContentSafetyDetection contentSafetyDetection,
        IDecisionService decisionService,
        IRejectionThresholdProvider rejectionThresholds)
    {
        _contentSafetyDetection = contentSafetyDetection;
        _decisionService = decisionService;
        _rejectionThresholds = rejectionThresholds;
    }

    async Task<Decision> IContentSafetyCommand.ModerateContentAsync(MediaType mediaType, string content, string[] blocklists)
    {
        // Detect/analyze content safety
        var detectionResult = await _contentSafetyDetection.ContentDetectionAsync(mediaType, content, blocklists);

        // Load the rejection thresholds settings
        var rejectThresholds = _rejectionThresholds.GetRejectionThresholds();

        // Make a decision based on the detection result and reject thresholds
        var decisionResult = _decisionService.MakeDecision(detectionResult, rejectThresholds);

        // Publish the decision to the event bus in infrastructure

        return decisionResult;
    }
}