using NotificationService.Application.Commands.CommandDto;
using NotificationService.Application.Commands.Interfaces;
using NotificationService.Application.EventDtos;
using NotificationService.Application.Helpers;

namespace NotificationService.Application.Services
{
    public class NotificationHandler : INotificationHandler
    {
        private readonly INotificationCommand _notificationCommand;
        private readonly IEventHandler _eventHandler;
        private readonly ISentNotificationCommand _sentNotificationCommand;

        public NotificationHandler(INotificationCommand notificationCommand, IEventHandler eventHandler, ISentNotificationCommand sentNotificationCommand)
        {
            _notificationCommand = notificationCommand;
            _eventHandler = eventHandler;
            _sentNotificationCommand = sentNotificationCommand;
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
                case EventNames.PostVoteCreated:
                    await HandlePostVoteCreated((PostVoteEventDto)dto);
                    break;
                case EventNames.PostRejected:
                    await HandlePostRejected((PostRejectedDto)dto);
                    break;
                case EventNames.ForumRejected:
                    await HandleForumRejected((ForumRejectedDto)dto);
                    break;
                case EventNames.CommentRejected:
                    await HandleCommentRejected((CommentRejectedDto)dto);
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
                var notificationId = await _notificationCommand.CreateNotificationAsync(message, dto.ForumId, "Forum", dto.PostId, "Post");

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
                    await _notificationCommand.CreateNotificationAsync(message, dto.PostId, "Post", dto.CommentId, "Comment");

                await _eventHandler.PostSubscribersRequested(notificationId, dto.PostId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private async Task HandlePostVoteCreated(PostVoteEventDto dto)
        {
            try
            {
                var message = MessageBuilder.BuildPostVoteCreated();

                var notificationId =
                    await _notificationCommand.CreateNotificationAsync(message, dto.PostId, "Post", 0, "Upvote");

                await _eventHandler.PostSubscribersRequested(notificationId, dto.PostId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private async Task HandlePostRejected(PostRejectedDto dto)
        {
            try
            {
                var message = MessageBuilder.BuildPostRejected();

                var notificationId =
                    await _notificationCommand.CreateNotificationAsync(message, dto.ForumId, "Forum", dto.PostId, "Post");

                await _sentNotificationCommand.CreateSentNotificationAsync(
                    new CreateSentNotificationDto(notificationId, dto.UserId));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private async Task HandleForumRejected(ForumRejectedDto dto)
        {
            try
            {
                var message = MessageBuilder.BuildForumRejected();

                var notificationId =
                    await _notificationCommand.CreateNotificationAsync(message, 0, "Forum", dto.ForumId, "Forum");

                await _sentNotificationCommand.CreateSentNotificationAsync(
                    new CreateSentNotificationDto(notificationId, dto.UserId));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private async Task HandleCommentRejected(CommentRejectedDto dto)
        {
            try
            {
                var message = MessageBuilder.BuildCommentRejected();

                var notificationId =
                    await _notificationCommand.CreateNotificationAsync(message, dto.PostId, "Post", dto.CommentId, "Comment");

                await _sentNotificationCommand.CreateSentNotificationAsync(
                    new CreateSentNotificationDto(notificationId, dto.UserId));
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