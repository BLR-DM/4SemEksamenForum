using NotificationService.Application.Commands.Interfaces;
using NotificationService.Application.EventDtos;
using NotificationService.Application.Helpers;

namespace NotificationService.Application.Services
{
    public class NotificationHandler : INotificatioHandler
    {
        private readonly INotificationCommand _command;
        private readonly IEventHandler _eventHandler;

        public NotificationHandler(INotificationCommand command, IEventHandler eventHandler)
        {
            _command = command;
            _eventHandler = eventHandler;
        }

        async Task INotificatioHandler.Handle(string topic, object dto)
        {
            if (topic == EventNames.PostPublished)
            {
                await HandlePostPublished((PostPublishedDto)dto);
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
    }

    public interface INotificatioHandler
    {
        Task Handle(string topic, object dto);
    }
}