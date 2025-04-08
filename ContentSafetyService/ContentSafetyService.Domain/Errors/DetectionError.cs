namespace ContentSafetyService.Domain.Errors;

/// <summary>
/// Class representing a detection error.
/// </summary>
public class DetectionError
{
    /// <summary>
    /// The error code.
    /// </summary>
    public string? code { get; set; }
    /// <summary>
    /// The error message.
    /// </summary>
    public string? message { get; set; }
    /// <summary>
    /// The error target.
    /// </summary>
    public string? target { get; set; }
    /// <summary>
    /// The error details.
    /// </summary>
    public string[]? details { get; set; }
    /// <summary>
    /// The inner error.
    /// </summary>
    public DetectionInnerError? innererror { get; set; }
}