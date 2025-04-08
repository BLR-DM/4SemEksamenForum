namespace ContentSafetyService.Domain;

/// <summary>
/// Class representing an image detection request.
/// </summary>
public class ImageDetectionRequest : DetectionRequest
{
    public Image Image { get; set; }

    /// <summary>
    /// Constructor for the ImageDetectionRequest class.
    /// </summary>
    /// <param name="content">The base64-encoded content of the image.</param>
    public ImageDetectionRequest(string content)
    {
        Image = new Image(content);
    }
}