namespace NotificationService.Application.EventDtos
{
    public record EventDto(string UserId, int? ForumId, int? PostId, int? CommentId);
}