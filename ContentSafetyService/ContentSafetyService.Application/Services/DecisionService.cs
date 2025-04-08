using ContentSafetyService.Domain;
using ContentSafetyService.Domain.Enums;
using Action = ContentSafetyService.Domain.Enums.Action;

namespace ContentSafetyService.Application.Services;

public class DecisionService : IDecisionService
{
    /// <summary>
    /// The valid threshold values.
    /// </summary>
    public static readonly int[] VALID_THRESHOLD_VALUES = { -1, 0, 2, 4, 6 };

    /// <summary>
    /// Makes a decision based on the detection result and the specified reject thresholds.
    /// Users can customize their decision-making method.
    /// </summary>
    /// <param name="detectionResult">The detection result object to make the decision on.</param>
    /// <param name="rejectThresholds">The reject thresholds for each category.</param>
    /// <returns>The decision made based on the detection result and the specified reject thresholds.</returns>

    Decision IDecisionService.MakeDecision(DetectionResult detectionResult, Dictionary<Category, int> rejectThresholds)
    {
        Dictionary<Category, Action> actionResult = new Dictionary<Category, Action>();
        Action finalAction = Action.Accept;

        foreach (KeyValuePair<Category, int> pair in rejectThresholds)
        {
            if (!VALID_THRESHOLD_VALUES.Contains(pair.Value))
            {
                throw new ArgumentException("RejectionThresholds can only be in (-1, 0, 2, 4, 6)");
            }

            int? severity = GetDetectionResultByCategory(pair.Key, detectionResult);
            if (severity == null)
            {
                throw new ArgumentException($"Can not find detection result for {pair.Key}");
            }

            Action action;
            if (pair.Value != -1 && severity >= pair.Value)
            {
                action = Action.Reject;
            }
            else
            {
                action = Action.Accept;
            }
            actionResult[pair.Key] = action;

            if (action.CompareTo(finalAction) > 0) // Reject > Accept in enum order (1 > 0)
            {
                finalAction = action;
            }
            // Could add Review as third option, so it would go as Accept, Review, Reject, making Reject override the rest
        }

        // blocklists
        if (detectionResult is TextDetectionResult textDetectionResult)
        {
            if (textDetectionResult.BlocklistsMatch != null &&
                textDetectionResult.BlocklistsMatch.Count > 0)
            {
                finalAction = Action.Reject;
            }
        }

        Console.WriteLine(finalAction);
        foreach (var res in actionResult)
        {
            Console.WriteLine($"Category: {res.Key}, Action: {res.Value}");
        }

        return new Decision(finalAction, actionResult);
    }

    /// <summary>
    /// Gets the severity score of the specified category from the given detection result.
    /// </summary>
    /// <param name="category">The category to get the severity score for.</param>
    /// <param name="detectionResult">The detection result object to retrieve the severity score from.</param>
    /// <returns>The severity score of the specified category.</returns>
    int? GetDetectionResultByCategory(Category category, DetectionResult detectionResult)
    {
        int? severityResult = null;
        if (detectionResult.CategoriesAnalysis != null)
        {
            foreach (var detailedResult in detectionResult.CategoriesAnalysis)
            {
                if (detailedResult.Category == category.ToString())
                {
                    severityResult = detailedResult.Severity;
                }
            }
        }

        return severityResult;
    }
}