namespace NotificationService.Application.Queries;

public record NotificationDto(int Id, string UserId, string Message, bool NotificationRead, DateTime CreatedAt);

//public class NotificationDto
//{
//    public string Message { get; set; }
//    public DateTime CreatedAt { get; set; }
//}