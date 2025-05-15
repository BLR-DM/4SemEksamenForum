using NotificationService.Application.Commands.Interfaces;
using NotificationService.Application.EventDtos;
using NotificationService.Application.Helpers;

namespace NotificationService.Application.Services
{
    public class NotificationHandler : INotificationHandler
    {
        private readonly INotificationCommand _command;
        private readonly IEventHandler _eventHandler;

        public NotificationHandler(INotificationCommand command, IEventHandler eventHandler)
        {
            _command = command;
            _eventHandler = eventHandler;
        }

        async Task INotificationHandler.Handle(string topic, object dto)
        {
            switch (topic)
            {
                case EventNames.PostPublished:
                    await HandlePostPublished((PostPublishedDto)dto);
                    break;
                case EventNames.CommentPublished:
                    await HandleCommentPublished((CommentPublishedDto)dto);
                    break;
                default:
                    throw new Exception($"Unknown topic: {topic}");
            }
        }

        private async Task HandlePostPublished(PostPublishedDto dto)
        {
            try
            {
                var message = MessageBuilder.BuildForPostPublished(dto);
                var notificationId = await _command.CreateNotificationAsync(message, dto.ForumId, "Forum", dto.PostId, "Post");

                await _eventHandler.ForumSubscribersRequested(notificationId, dto.ForumId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private async Task HandleCommentPublished(CommentPublishedDto dto)
        {
            try
            {
                var message = MessageBuilder.BuildForCommentPublished(dto);

                var notificationId =
                    await _command.CreateNotificationAsync(message, dto.PostId, "Post", dto.CommentId, "Comment");

                await _eventHandler.PostSubscribersRequested(notificationId, dto.PostId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }

    public interface INotificationHandler
    {
        Task Handle(string topic, object dto);
    }
}