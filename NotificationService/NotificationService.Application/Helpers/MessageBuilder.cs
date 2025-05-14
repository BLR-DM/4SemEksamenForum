using NotificationService.Application.EventDtos;

namespace NotificationService.Application.Helpers
{
    public static class MessageBuilder
    {
        public static string BuildForPostPublished(PostPublishedDto dto)
        {
            return $"A new post have been created on forum: {dto.ForumName}";
        }
    }
}