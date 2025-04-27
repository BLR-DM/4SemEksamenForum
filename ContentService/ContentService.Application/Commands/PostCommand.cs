using ContentService.Application.Commands.CommandDto.CommentDto;
using ContentService.Application.Commands.CommandDto.PostDto;
using ContentService.Application.Commands.Interfaces;
using ContentService.Application.Helpers;
using ContentService.Application.Services;
using ContentService.Domain.Entities;
using ContentService.Domain.Enums;

namespace ContentService.Application.Commands
{
    public class PostCommand : IPostCommand
    {
        private readonly IForumRepository _forumRepository;
        private readonly IEventHandler _eventHandler;

        private readonly IUnitOfWork _unitOfWork;

        public PostCommand(IUnitOfWork unitOfWork, IForumRepository forumRepository, IEventHandler eventHandler)
        {
            _unitOfWork = unitOfWork;
            _forumRepository = forumRepository;
            _eventHandler = eventHandler;
        }

        async Task IPostCommand.CreateCommentAsync(CreateCommentDto commentDto, string username, int postId,
            string appUserId, int forumId)
        {
            try
            {
                await _unitOfWork.BeginTransaction();

                // Load
                var forum = await _forumRepository.GetForumWithSinglePostAsync(forumId, postId);
                var post = forum.GetPostById(postId);

                // Do
                var comment = post.CreateComment(username, commentDto.Content, appUserId);

                // Save
                await _unitOfWork.Commit();

                // Event
                var commentId = ContentIdFormatter.FormatCommentId(forum.Id, post.Id, comment.Id);
                await _eventHandler.CommentSubmitted(commentId, comment.Content);
            }
            catch (Exception)
            {
                await _unitOfWork.Rollback();
                throw;
            }
        }

        async Task IPostCommand.HandleCommentApprovalAsync(PublishCommentDto commentDto)
        {
            try
            {
                await _unitOfWork.BeginTransaction();

                // Load
                var forum = await _forumRepository.GetForumWithSinglePostAsync(commentDto.ForumId, commentDto.PostId);
                var post = forum.GetPostById(commentDto.PostId);
                var comment = post.GetCommentById(commentDto.CommentId);

                // Idempotency check
                if (comment.Status == Status.Published)
                    return;

                // Do
                comment.MarkAsPublished();

                // Event
                await _eventHandler.CommentPublished(post.AppUserId, forum.Id, post.Id, comment.Id);

                // Save
                await _unitOfWork.Commit();
            }
            catch (Exception)
            {
                await _unitOfWork.Rollback();
                throw;
            }
        }

        async Task IPostCommand.HandleCommentRejectionAsync(RejectCommentDto commentDto)
        {
            try
            {
                await _unitOfWork.BeginTransaction();

                // Load
                var forum = await _forumRepository.GetForumWithSinglePostAsync(commentDto.ForumId, commentDto.PostId);
                var post = forum.GetPostById(commentDto.PostId);
                var comment = post.GetCommentById(commentDto.CommentId);

                // Idempotency check
                if (comment.Status == Status.Rejected)
                    return;

                // Do
                comment.MarkAsRejected();

                // Event
                await _eventHandler.CommentRejected(post.AppUserId, forum.Id, post.Id, comment.Id);

                // Save
                await _unitOfWork.Commit();
            }
            catch (Exception)
            {
                await _unitOfWork.Rollback();
                throw;
            }
        }

        async Task IPostCommand.UpdateCommentAsync(UpdateCommentDto commentDto, string appUserId, int forumId,
            int postId, int commentId)
        {
            try
            {
                await _unitOfWork.BeginTransaction();

                // Load
                var forum = await _forumRepository.GetForumWithSinglePostAsync(forumId, postId);
                var post = forum.GetPostById(postId);

                // Do
                var comment = post.UpdateComment(commentId, commentDto.Content, appUserId);
                _forumRepository.UpdateComment(comment, commentDto.RowVersion);

                // Save
                await _unitOfWork.Commit();
            }
            catch (Exception)
            {
                await _unitOfWork.Rollback();
                throw;
            }
        }

        async Task IPostCommand.DeleteCommentAsync(DeleteCommentDto commentDto, string appUserId, int forumId,
            int postId, int commentId)
        {
            try
            {
                await _unitOfWork.BeginTransaction();

                // Load
                var forum = await _forumRepository.GetForumWithSinglePostAsync(forumId, postId);
                var post = forum.GetPostById(postId);

                // Do
                var comment = post.DeleteComment(commentId, appUserId);
                _forumRepository.DeleteComment(comment, commentDto.RowVersion);

                // Save
                await _unitOfWork.Commit();

                await _eventHandler.CommentDeleted(forum.Id, post.Id, comment.Id);
            }
            catch (Exception)
            {
                await _unitOfWork.Rollback();
                throw;
            }
        }
    }
}
