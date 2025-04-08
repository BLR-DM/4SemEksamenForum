namespace ContentSafetyService.Domain;

/// <summary>
/// Base class for detection result.
/// </summary>
public class DetectionResult
{
    /// <summary>
    /// The detailed result for categories analysis.
    /// </summary>
    public List<CategoriesAnalysis>? CategoriesAnalysis { get; set; }
}