namespace NotificationService.Application.Queries;

public record NotificationDto(string Message, DateTime CreatedAt);

//public class NotificationDto
//{
//    public string Message { get; set; }
//    public DateTime CreatedAt { get; set; }
//}