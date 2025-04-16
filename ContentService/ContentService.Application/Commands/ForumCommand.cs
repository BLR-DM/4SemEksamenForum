using ContentService.Application.Commands.CommandDto.ForumDto;
using ContentService.Application.Commands.CommandDto.PostDto;
using ContentService.Application.Commands.Interfaces;
using ContentService.Application.Services;
using ContentService.Domain.Entities;
using ContentService.Domain.Enums;
using Dapr.Client;
using Microsoft.Extensions.Hosting;

namespace ContentService.Application.Commands
{
    public class ForumCommand : IForumCommand
    {
        private readonly IForumRepository _forumRepository;
        private readonly IEventHandler _eventHandler;
        private readonly IUnitOfWork _unitOfWork;

        public ForumCommand(IUnitOfWork unitOfWork, IForumRepository forumRepository, IEventHandler eventHandler )
        {
            _unitOfWork = unitOfWork;
            _forumRepository = forumRepository;
            _eventHandler = eventHandler;
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

                //var test = forum.Id; // <- Works. Gets the newly created Id, need for publishing
                /*
                INSERT INTO "Forums" ("AppUserId", "CreatedDate", "ForumName", "Status")
                VALUES (@p0, @p1, @p2, @p3)
                RETURNING "Id", xmin;
                */

                // Need to moderate content, not forumName <- just for test
                //var contentModerationDto = new ContentModerationDto(forum.Id, forum.Content);

                // Testing publish -> should not be here
                //await _daprClient.PublishEventAsync("pubsub", "contentSubmitted", contentModerationDto);

                // Publish event
                var contentId = $"Forum:{forum.Id}";
                await _eventHandler.ContentSubmitted(contentId, forum.Content);
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
                if (forum.Status == Status.Published)
                    return;

                // Do
                forum.Publish();

                // Publish event
                await _eventHandler.ForumPublished(forum.AppUserId, forum.Id);

                // Save
                await _unitOfWork.Commit();
            }
            catch (Exception)
            {
                await _unitOfWork.Rollback();
                throw;
            }
        }

        //async Task IForumCommand.HandlePublishAsync(PublishForumDto forumDto)
        //{
        //    try
        //    {
        //        await _unitOfWork.BeginTransaction();

        //        // Load
        //        var forum = await _forumRepository.GetForumAsync(forumDto.Id);

        //        // Idempotency check
        //        if (forum.Status == Status.Published)
        //            return;

        //        // Publish event
        //        // await _dapr.PublishEventAsync("pubsub", "forumPublished", forum);

        //        // Do
        //        forum.Publish();

        //        // Save
        //        await _unitOfWork.Commit();
        //    }
        //    catch (Exception)
        //    {
        //        await _unitOfWork.Rollback();
        //        throw;
        //    }
        //}

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
                await _unitOfWork.BeginTransaction();

                // Load
                var forum = await _forumRepository.GetForumAsync(forumId);

                // Do
                var post = forum.AddPost(postDto.Title, postDto.Content, username, appUserId);

                // Save
                await _unitOfWork.Commit();

                // Publish event
                var contentId = $"Post:{forum.Id}-{post.Id}";
                await _eventHandler.ContentSubmitted(contentId, post.Content);
                //await _eventHandler.PostSubmitted(post.Id, post.Content);
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

        Task IForumCommand.HandlePublishAsync(PublishForumDto forumDto)
        {
            throw new NotImplementedException();
        }
    }
}
