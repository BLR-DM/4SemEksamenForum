namespace NotificationService.Domain.Entities;

public class Notification
{
    public int Id { get; protected set; }
    public string UserId { get; protected set; }
    public string Message { get; protected set; }
    public bool IsRead { get; protected set; }
    public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow.AddHours(2);

    protected Notification() { }

    private Notification(string userId, string message)
    {
        UserId = userId;
        Message = message;
    }

    public static Notification Create(string userId, string message)
    {
        return new Notification(userId, message);
    }

    public void MarkAsRead()
    {
        // Check userId as creator
        IsRead = true;
    }
}