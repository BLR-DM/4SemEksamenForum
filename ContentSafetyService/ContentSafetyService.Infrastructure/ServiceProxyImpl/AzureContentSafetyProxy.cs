using ContentSafetyService.Domain;
using ContentSafetyService.Domain.Enums;
using ContentSafetyService.Domain.Errors;
using System.Text.Json;
using System.Text.Json.Serialization;
using ContentSafetyService.Domain.Exceptions;

namespace ContentSafetyService.Infrastructure.ServiceProxyImpl;

public class AzureContentSafetyProxy : IAzureContentSafetyProxy
{
    /// <summary>
    /// The HTTP client.
    /// </summary>
    private readonly HttpClient _client;

    /// <summary>
    /// The JSON serializer options.
    /// </summary>
    private readonly JsonSerializerOptions options =
        new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters = { new JsonStringEnumConverter() }
        };

    public AzureContentSafetyProxy(HttpClient client)
    {
        _client = client;
    }

    /// <summary>
    /// Detects unsafe content using the Content Safety API.
    /// </summary>
    /// <param name="mediaType">The media type of the content to detect.</param>
    /// <param name="content">The content to detect.</param>
    /// <param name="blocklists">The blocklists to use for text detection.</param>
    /// <returns>The response from the Content Safety API.</returns>
    async Task<DetectionResult> IAzureContentSafetyProxy.DetectAsync(HttpRequestMessage msg, MediaType mediaType)
    {
        var response = await _client.SendAsync(msg);
        var responseText = await response.Content.ReadAsStringAsync();

        // test
        Console.WriteLine((int)response.StatusCode);
        foreach (var header in response.Headers)
        {
            Console.WriteLine($"{header.Key}: {string.Join(", ", header.Value)}");
        }
        Console.WriteLine(responseText);

        if (!response.IsSuccessStatusCode)
        {
            DetectionErrorResponse? error =
                JsonSerializer.Deserialize<DetectionErrorResponse>(responseText, options);
            if (error == null || error.error == null || error.error.code == null || error.error.message == null)
            {
                throw new DetectionException(response.StatusCode.ToString(),
                    $"Error is null. Response text is {responseText}");
            }
            throw new DetectionException(error.error.code, error.error.message);
        }

        var result = DeserializeDetectionResult(responseText, mediaType);
        if (result == null)
        {
            throw new DetectionException(response.StatusCode.ToString(),
                $"HttpResponse is null. Response text is {responseText}");
        }

        return result;
    }

    /// <summary>
    /// Deserializes the JSON string into a DetectionResult object based on the media type.
    /// </summary>
    /// <param name="json">The JSON string to deserialize.</param>
    /// <param name="mediaType">The media type of the detection result.</param>
    /// <returns>The deserialized DetectionResult object for the Content Safety API response.</returns>
    DetectionResult? DeserializeDetectionResult(string json, MediaType mediaType)
    {
        switch (mediaType)
        {
            case MediaType.Text:
                return JsonSerializer.Deserialize<TextDetectionResult>(json, options);
            case MediaType.Image:
                return JsonSerializer.Deserialize<ImageDetectionResult>(json, options);
            default:
                throw new ArgumentException($"Invalid Media Type {mediaType}");
        }
    }
}