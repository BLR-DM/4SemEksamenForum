namespace NotificationService.Domain.Entities;

public class Notification
{
    public int Id { get; protected set; }
    public string Message { get; protected set; }
    public int SourceId { get; protected set; }
    public string SourceType { get; protected set; }
    public int ContextId { get; protected set; }
    public string ContextType { get; protected set; }
    public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow.AddHours(2);

    protected Notification() { }

    private Notification(string message, int sourceId, string sourceType, int contextId, string contextType)
    {
        Message = message;
        SourceId = sourceId;
        SourceType = sourceType;
        ContextId = contextId;
        ContextType = contextType;
    }

    public static Notification Create(string message, int sourceId, string sourceType, int contextId, string contextType)
    {
        return new Notification(message, sourceId, sourceType, contextId, contextType);
    }

}