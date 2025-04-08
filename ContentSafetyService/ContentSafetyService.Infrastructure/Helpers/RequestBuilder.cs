using ContentSafetyService.Domain;
using ContentSafetyService.Domain.Enums;
using ContentSafetyService.Infrastructure.Configuration;
using ContentSafetyService.Infrastructure.Services;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;

namespace ContentSafetyService.Infrastructure.Helpers;

public class RequestBuilder : IRequestBuilder
{
    private readonly string _apiVersion;
    private readonly string _endpoint;
    private readonly string _subscriptionKey;

    /// <summary>
    /// The JSON serializer options.
    /// </summary>
    private readonly JsonSerializerOptions options =
        new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters = { new JsonStringEnumConverter() }
        };

    public RequestBuilder(IOptions<ContentSafetySettings> settings)
    {
        _apiVersion = settings.Value.ApiVersion;
        _endpoint = settings.Value.Endpoint;
        _subscriptionKey = settings.Value.SubscriptionKey;
    }

    HttpRequestMessage IRequestBuilder.BuildHttpRequestMessage(MediaType mediaType, string content, string[] blocklists)
    {
        var url = BuildUrl(mediaType);
        var requestBody = BuildRequestBody(mediaType, content, blocklists);
        var payload = JsonSerializer.Serialize(requestBody, requestBody.GetType(), options);
        var msg = new HttpRequestMessage(HttpMethod.Post, url);

        msg.Content = new StringContent(payload, Encoding.UTF8, "application/json");
        msg.Headers.Add("Ocp-Apim-Subscription-Key", _subscriptionKey);

        return msg;
    }

    /// <summary>
    /// Builds the URL for the Content Safety API based on the media type.
    /// </summary>
    /// <param name="mediaType">The type of media to analyze.</param>
    /// <returns>The URL for the Content Safety API.</returns>
    string BuildUrl(MediaType mediaType)
    {
        switch (mediaType)
        {
            case MediaType.Text:
                return $"{_endpoint}/contentsafety/text:analyze?api-version={_apiVersion}";
            case MediaType.Image:
                return $"{_endpoint}/contentsafety/image:analyze?api-version={_apiVersion}";
            default:
                throw new ArgumentException($"Invalid Media Type {mediaType}");
        }
    }

    /// <summary>
    /// Builds the request body for the Content Safety API request.
    /// </summary>
    /// <param name="mediaType">The type of media to analyze.</param>
    /// <param name="content">The content to analyze.</param>
    /// <param name="blocklists">The blocklists to use for text analysis.</param>
    /// <returns>The request body for the Content Safety API request.</returns>
    DetectionRequest BuildRequestBody(MediaType mediaType, string content, string[] blocklists)
    {
        switch (mediaType)
        {
            case MediaType.Text:
                return new TextDetectionRequest(content, blocklists);
            case MediaType.Image:
                return new ImageDetectionRequest(content);
            default:
                throw new ArgumentException($"Invalid Media Type {mediaType}");
        }
    }
}