namespace ContentSafetyService.Domain;

/// <summary>
/// Class representing a text detection request.
/// </summary>
public class TextDetectionRequest : DetectionRequest
{
    public string Text { get; set; }
    public string[] BlocklistNames { get; set; }

    /// <summary>
    /// Constructor for the TextDetectionRequest class.
    /// </summary>
    /// <param name="text">The text to be detected.</param>
    /// <param name="blocklistNames">The names of the blocklists to use for detecting the text.</param>
    public TextDetectionRequest(string text, string[] blocklistNames)
    {
        Text = text;
        BlocklistNames = blocklistNames;
    }
}