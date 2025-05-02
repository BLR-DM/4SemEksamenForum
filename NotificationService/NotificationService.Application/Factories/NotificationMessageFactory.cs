using System.Text.Json;
using NotificationService.Application.EventDtos;
using NotificationService.Application.Factories.Interfaces;

namespace NotificationService.Application.Factories
{
    public class NotificationMessageFactory : INotificationMessageFactory
    {
        async Task<string> INotificationMessageFactory.BuildMessageAsync(string topic, EventDto dto)
        {
            return topic switch
            { 
               "post-rejected" => await BuildForPostRejected(dto),
                "comment-rejected" => await BuildForCommentRejected(dto),

                _ => await Task.FromResult("Unknown event")
            };

        }

        public string BuildForPostPublished(RequestedForumSubscribersCollectedEventDto dto)
        {
            return $"A post with ID {dto.PostId} has been published in forum {dto.ForumId}";
        }

        private Task<string> BuildForCommentPublished(EventDto dto)
        {
            return Task.FromResult($"A comment with ID {dto.CommentId} has been published on a post with ID {dto.PostId}");
        }

        private Task<string> BuildForPostRejected(EventDto dto)
        {
            return Task.FromResult($"Your post with ID {dto.PostId} has been rejected");
        }

        private Task<string> BuildForCommentRejected(EventDto dto)
        {
            return Task.FromResult($"Your comment with ID {dto.CommentId} has been rejected");
        }

        private Task<string> BuildForPostUpvoteCreated(EventDto dto)
        {
            return Task.FromResult($"Your post with ID {dto.PostId} has been liked");
        }

        private Task<string> BuildForPostDownVoteCreated(EventDto dto)
        {
            return Task.FromResult($"Your post with ID {dto.PostId} has been disliked");
        }

        private Task<string> BuildForCommentUpvoteCreated(EventDto dto)
        {
            return Task.FromResult($"Your comment with ID {dto.CommentId} has been liked");
        }

        private Task<string> BuildForCommentDownvoteCreated(EventDto dto)
        {
            return Task.FromResult($"Your comment with ID {dto.CommentId} has been disliked");
        }

    }
}