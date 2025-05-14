namespace NotificationService.Application.Queries;

public record NotificationDto()
{
    public int Id { get; set; }
    public string Message { get; set; }
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; }
    public int SourceId { get; set; }
    public string SourceType { get; set; }
    public int ContextId { get; set; }
    public string ContextType { get; set; }
};

public record SentNotification(int NotificationId, string UserId, bool IsRead);