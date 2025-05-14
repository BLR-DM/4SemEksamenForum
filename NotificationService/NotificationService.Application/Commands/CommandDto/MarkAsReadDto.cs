namespace NotificationService.Application.Commands.CommandDto
{
    public record MarkAsReadDto(string UserId, int NotificationId);
}