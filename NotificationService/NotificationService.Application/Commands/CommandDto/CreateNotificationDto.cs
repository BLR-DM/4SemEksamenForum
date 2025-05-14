namespace NotificationService.Application.Commands.CommandDto;

public record CreateNotificationDto(string Message, int SourceId, string SourceType, int ContextId, string ContextType);