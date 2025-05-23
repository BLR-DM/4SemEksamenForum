﻿using ContentService.Application.Commands.CommandDto.ForumDto;
using ContentService.Application.Commands.CommandDto.PostDto;
using ContentService.Application.Commands.Interfaces;
using ContentService.Application.Helpers;
using ContentService.Application.Services;
using ContentService.Domain.Entities;
using ContentService.Domain.Enums;
using Microsoft.AspNetCore.Http;

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

        async Task IForumCommand.CreateForumAsync(CreateForumDto forumDto, string appUserId)
        {
            try
            {
                await _unitOfWork.BeginTransaction();

                // Load
                var otherForums = await _forumRepository.GetForumsAsync();

                // Do
                var forum = Forum.Create(forumDto.ForumName, forumDto.Content, appUserId, otherForums);

                // Save
                await _forumRepository.AddForumAsync(forum);
                await _unitOfWork.Commit();

                // Event
                var forumId = ContentIdFormatter.FormatForumId(forum.Id);
                await _eventHandler.ForumSubmitted(forumId, forum.Content);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await _unitOfWork.Rollback();
                throw;
            }
        }

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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await _unitOfWork.Rollback();
                throw;
            }
        }

        public async Task HandleForumRejectionAsync(RejectForumDto forumDto)
        {
            try
            {
                await _unitOfWork.BeginTransaction();

                // Load
                var forum = await _forumRepository.GetForumOnlyAsync(forumDto.Id);

                // Idempotency check
                if (forum.Status == Status.Rejected)
                    return;

                // Do
                forum.MarkAsRejected();

                // Event
                await _eventHandler.ForumRejected(forum.AppUserId, forum.Id);

                // Save
                await _unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
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
                var forum = await _forumRepository.GetForumWithPostsAsync(postDto.ForumId);

                var post = forum.GetPostById(postDto.PostId);

                // Idempotency check
                if (post.Status == Status.Published)
                    return;

                // Do
                post.MarkAsPublished();

                // Event
                await _eventHandler.PostPublished(post.AppUserId, forum.Id, post.Id, forum.ForumName);

                // Save
                await _unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await _unitOfWork.Rollback();
                throw;
            }
        }

        async Task IForumCommand.HandlePostRejectionAsync(RejectPostDto postDto)
        {
            try
            {
                await _unitOfWork.BeginTransaction();

                // Load
                var forum = await _forumRepository.GetForumWithPostsAsync(postDto.ForumId);

                var post = forum.GetPostById(postDto.PostId);

                // Idempotency check
                if (post.Status == Status.Rejected)
                    return;

                // Do
                post.MarkAsRejected();

                // Event
                await _eventHandler.PostRejected(post.AppUserId, forum.Id, post.Id);

                // Save
                await _unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
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
                var forum = await _forumRepository.GetForumOnlyAsync(forumId);

                // Do
                forum.Update(forumDto.Content, appUserId);
                _forumRepository.UpdateForumAsync(forum, forumDto.RowVersion);

                // Save
                await _unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await _unitOfWork.Rollback();
                throw;
            }
        }

        async Task IForumCommand.DeleteForumAsync(string appUserId, int forumId)
        {
            try
            {
                await _unitOfWork.BeginTransaction();

                var forum = await _forumRepository.GetForumWithAllAsync(forumId);

                forum.AssureUserIsCreator(appUserId);

                var deletedPosts = forum.DeleteAllPosts();
                var deletedComments = deletedPosts.SelectMany(p => p.DeleteAllComments()).ToList();

                _forumRepository.DeleteComments(deletedComments);
                _forumRepository.DeletePosts(deletedPosts);
                _forumRepository.DeleteForum(forum);

                await _unitOfWork.Commit();

                // Event
                await _eventHandler.ForumDeleted(forum.AppUserId, forum.Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await _unitOfWork.Rollback();
                throw;
            }
        }

        async Task IForumCommand.DeleteForumModAsync(int forumId)
        {
            try
            {
                await _unitOfWork.BeginTransaction();

                var forum = await _forumRepository.GetForumWithAllAsync(forumId);

                var deletedPosts = forum.DeleteAllPosts();
                var deletedComments = deletedPosts.SelectMany(p => p.DeleteAllComments()).ToList();

                _forumRepository.DeleteComments(deletedComments);
                _forumRepository.DeletePosts(deletedPosts);
                _forumRepository.DeleteForum(forum);

                await _unitOfWork.Commit();

                // Event
                await _eventHandler.ForumDeleted(forum.AppUserId, forum.Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
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
                var postId = ContentIdFormatter.FormatPostId(forum.Id, post.Id);
                await _eventHandler.PostSubmitted(postId, post.Content);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
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
                var forum = await _forumRepository.GetForumWithPostsAsync(forumId);

                // Do
                var post = forum.UpdatePost(postId, postDto.Title, postDto.Content, appUserId);
                _forumRepository.UpdatePost(post, postDto.RowVersion);

                //Save
                await _unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await _unitOfWork.Rollback();
                throw;
            }
        }

        async Task IForumCommand.DeletePostAsync(string appUserId, int forumId, int postId)
        {
            try
            {
                await _unitOfWork.BeginTransaction();

                // Load
                var forum = await _forumRepository.GetForumWithSinglePostAsync(forumId, postId);

                // Do
                var post = forum.DeletePost(postId, appUserId, out var deletedComments);
                if (post == null)
                {
                    Console.WriteLine("Post already deleted or not found.");
                    await _unitOfWork.Commit();
                    return;
                }

                _forumRepository.DeletePost(post);
                _forumRepository.DeleteComments(deletedComments);

                //Save
                await _unitOfWork.Commit();

                await _eventHandler.PostDeleted(appUserId, forum.Id, post.Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await _unitOfWork.Rollback();
                throw;
            }
        }

        async Task IForumCommand.DeletePostModAsync(int forumId, int postId)
        {
            try
            {
                await _unitOfWork.BeginTransaction();

                // Load
                var forum = await _forumRepository.GetForumWithSinglePostAsync(forumId, postId);
                var post = forum.GetPostById(postId);

                // Do
                var deletedComments = post.DeleteAllComments();

                _forumRepository.DeletePost(post);
                _forumRepository.DeleteComments(deletedComments);

                //Save
                await _unitOfWork.Commit();

                await _eventHandler.PostDeleted(post.AppUserId, forum.Id, post.Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await _unitOfWork.Rollback();
                throw;
            }
        }
    }
}
