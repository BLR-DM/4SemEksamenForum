using ContentService.Application.Commands.CommandDto.ForumDto;
using ContentService.Application.Commands.CommandDto.PostDto;
using ContentService.Application.Commands.Interfaces;
using ContentService.Domain.Entities;
using ContentService.Domain.Enums;
using Dapr.Client;

namespace ContentService.Application.Commands
{
    public class ForumCommand : IForumCommand
    {
        private readonly IForumRepository _forumRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly DaprClient _daprClient;

        public ForumCommand(IUnitOfWork unitOfWork, IForumRepository forumRepository, DaprClient daprClient)
        {
            _unitOfWork = unitOfWork;
            _forumRepository = forumRepository;
            _daprClient = daprClient;
        }

        public record ContentModerationDto(int Id, string Content);

        async Task IForumCommand.CreateForumAsync(CreateForumDto forumDto, string appUserId)
        {
            try
            {
                var forum = Forum.Create(forumDto.ForumName, forumDto.Content, appUserId);

                await _unitOfWork.BeginTransaction();

                // Do
                await _forumRepository.AddForumAsync(forum);

                // Save
                await _unitOfWork.Commit();

                var test = forum.Id; // <- Works. Gets the newly created Id, need for publishing
                /*
                INSERT INTO "Forums" ("AppUserId", "CreatedDate", "ForumName", "Status")
                VALUES (@p0, @p1, @p2, @p3)
                RETURNING "Id", xmin;
                */

                // Need to moderate content, not forumName <- just for test
                var contentModerationDto = new ContentModerationDto(forum.Id, forum.Content);

                // Testing publish -> should not be here
                await _daprClient.PublishEventAsync("pubsub", "contentSubmitted", contentModerationDto);
            }
            catch (Exception)
            {
                await _unitOfWork.Rollback();
                throw;
            }
        }

        //[Topic("pubsub", "forumApproved")]
        async Task IForumCommand.HandleApprovalAsync(PublishForumDto forumDto)
        {
            try
            {
                await _unitOfWork.BeginTransaction();
                
                // Load
                var forum = await _forumRepository.GetForumAsync(forumDto.Id);

                // Idempotency check
                if (forum.Status == Status.Approved)
                    return;

                // Do
                forum.Approve();

                // Publish event
                // await _dapr.PublishEventAsync("pubsub", "forumReadyToPublish", forum.Id);

                // Save
                await _unitOfWork.Commit();
            }
            catch (Exception)
            {
                await _unitOfWork.Rollback();
                throw;
            }
        }

        //[Topic("pubsub", "forumToPublish")]
        async Task IForumCommand.HandlePublishAsync(PublishForumDto forumDto)
        {
            try
            {
                await _unitOfWork.BeginTransaction();

                // Load
                var forum = await _forumRepository.GetForumAsync(forumDto.Id);

                // Idempotency check
                if (forum.Status == Status.Published)
                    return;

                // Publish event
                // await _dapr.PublishEventAsync("pubsub", "forumPublished", forum);

                // Do
                forum.Publish();

                // Save
                await _unitOfWork.Commit();
            }
            catch (Exception)
            {
                await _unitOfWork.Rollback();
                throw;
            }
        }

        async Task IForumCommand.UpdateForumAsync(UpdateForumDto forumDto, string appUserId, int forumId)
        {
            try
            {
                await _unitOfWork.BeginTransaction();

                // Load
                var forum = await _forumRepository.GetForumAsync(forumId);

                // Do
                forum.Update(forumDto.Content, appUserId);
                _forumRepository.UpdateForumAsync(forum, forumDto.RowVersion);

                // Save
                await _unitOfWork.Commit();
            }
            catch (Exception)
            {
                await _unitOfWork.Rollback();
                throw;
            }
        }

        async Task IForumCommand.DeleteForumAsync(DeleteForumDto forumDto, int forumId)
        {
            try
            {
                await _unitOfWork.BeginTransaction();

                // Load
                var forum = await _forumRepository.GetForumAsync(forumId);

                // Do
                _forumRepository.DeleteForum(forum, forumDto.RowVersion);

                // Save
                await _unitOfWork.Commit();
            }
            catch (Exception)
            {
                await _unitOfWork.Rollback();
                throw;
            }
        }

        async Task IForumCommand.CreatePostAsync(CreatePostDto postDto, string username, string appUserId, int forumId)
        {
            try
            {
                // await _unitOfWork.BeginTransaction();

                // Load
                var forum = await _forumRepository.GetForumAsync(forumId);

                // Do
                forum.AddPost(postDto.Title, postDto.Content, username, appUserId);

                //Save
                await _unitOfWork.Commit();
            }
            catch (Exception)
            {
                await _unitOfWork.Rollback();
                throw;
            }
        }

        async Task IForumCommand.UpdatePostAsync(UpdatePostDto postDto, string appUserId, int forumId, int postId)
        {
            try
            {
                await _unitOfWork.BeginTransaction();

                // Load
                var forum = await _forumRepository.GetForumAsync(forumId);

                // Do
                var post = forum.UpdatePost(postId, postDto.Title, postDto.Content, appUserId);
                _forumRepository.UpdatePost(post, postDto.RowVersion);

                //Save
                await _unitOfWork.Commit();
            }
            catch (Exception)
            {
                await _unitOfWork.Rollback();
                throw;
            }
        }

        async Task IForumCommand.DeletePostAsync(DeletePostDto postDto, string appUserId, int forumId, int postId)
        {
            try
            {
                // await _unitOfWork.BeginTransaction();

                // Load
                var forum = await _forumRepository.GetForumAsync(forumId);

                // Do
                var post = forum.DeletePost(postId, appUserId);
                _forumRepository.DeletePost(post, postDto.RowVersion);

                //Save
                await _unitOfWork.Commit();
            }
            catch (Exception)
            {
                await _unitOfWork.Rollback();
                throw;
            }
        }
    }
}
