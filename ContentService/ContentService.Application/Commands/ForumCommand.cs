﻿using ContentService.Application.Commands.CommandDto.ForumDto;
using ContentService.Application.Commands.CommandDto.PostDto;
using ContentService.Application.Commands.Interfaces;
using ContentService.Application.Helpers;
using ContentService.Application.Services;
using ContentService.Domain.Entities;
using ContentService.Domain.Enums;

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

                // Event
                var contentId = ContentIdFormatter.FormatForumId(forum.Id);
                await _eventHandler.ContentSubmitted(contentId, forum.Content);
            }
            catch (Exception)
            {
                await _unitOfWork.Rollback();
                throw;
            }
        }

        //[Topic("pubsub", "forumApproved")]
        async Task IForumCommand.HandleForumApprovalAsync(PublishForumDto forumDto)
        {
            try
            {
                await _unitOfWork.BeginTransaction();
                
                // Load
                var forum = await _forumRepository.GetForumOnlyAsync(forumDto.Id);

                // Idempotency check
                if (forum.Status == Status.Published)
                    return;

                // Do
                forum.MarkAsPublished();

                // Event
                await _eventHandler.ForumPublished(forum.AppUserId, forum.Id);
                // Subject to change? What if can't publish / save? Deadletter?
                // Save
                await _unitOfWork.Commit();
            }
            catch (Exception)
            {
                await _unitOfWork.Rollback();
                throw;
            }
        }

        async Task IForumCommand.HandlePostApprovalAsync(PublishPostDto postDto)
        {
            try
            {
                await _unitOfWork.BeginTransaction();

                // Load
                var forum = await _forumRepository.GetForumAsync(postDto.ForumId);

                var post = forum.GetPostById(postDto.PostId);

                // Idempotency check
                if (post.Status == Status.Published)
                    return;

                // Do
                post.MarkAsPublished();

                // Event
                await _eventHandler.PostPublished(post.AppUserId, forum.Id, post.Id);

                // Save
                await _unitOfWork.Commit();
            }
            catch (Exception)
            {
                await _unitOfWork.Rollback();
                throw;
            }
        }

        public async Task HandleRejectionAsync(RejectForumDto forumDto)
        {
            try
            {
                await _unitOfWork.BeginTransaction();

                // Load
                var forum = await _forumRepository.GetForumOnlyAsync(forumDto.Id);

                // Do
                forum.MarkAsRejected();

                // Save
                await _unitOfWork.Commit();

                //await _eventHandler.ForumRejected(forum.AppUserId, forum.Id);
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

        //        // MarkAsPublished event
        //        // await _dapr.PublishEventAsync("pubsub", "forumPublished", forum);

        //        // Do
        //        forum.MarkAsPublished();

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
                var forum = await _forumRepository.GetForumOnlyAsync(forumId);

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
                var forum = await _forumRepository.GetForumOnlyAsync(forumId);

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
                var forum = await _forumRepository.GetForumOnlyAsync(forumId);

                // Do
                var post = forum.AddPost(postDto.Title, postDto.Content, username, appUserId);

                // Save
                await _unitOfWork.Commit();

                // Event
                var contentId = ContentIdFormatter.FormatPostId(forum.Id, post.Id);
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
    }
}
