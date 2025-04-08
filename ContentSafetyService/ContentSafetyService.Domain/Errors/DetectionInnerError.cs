namespace ContentSafetyService.Domain.Errors;

/// <summary>
/// Class representing a detection inner error.
/// </summary>
public class DetectionInnerError
{
    /// <summary>
    /// The inner error code.
    /// </summary>
    public string? code { get; set; }
    /// <summary>
    /// The inner error message.
    /// </summary>
    public string? innererror { get; set; }
}