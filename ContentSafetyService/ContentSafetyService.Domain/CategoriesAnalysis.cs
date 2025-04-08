namespace ContentSafetyService.Domain;

/// <summary>
/// Class representing a detailed detection result for a specific category.
/// </summary>
public class CategoriesAnalysis
{
    /// <summary>
    /// The category of the detection result.
    /// </summary>
    public string? Category { get; set; }
    /// <summary>
    /// The severity of the detection result.
    /// </summary>
    public int? Severity { get; set; }
}