using ContentService.Application.Commands.CommandDto.CommentDto;
using ContentService.Application.Commands.Interfaces;

namespace ContentService.Application.Commands
{
    public class PostCommand : IPostCommand
    {
        private readonly IForumRepository _forumRepository;

        private readonly IUnitOfWork _unitOfWork;

        public PostCommand(IUnitOfWork unitOfWork, IForumRepository forumRepository)
        {
            _unitOfWork = unitOfWork;
            _forumRepository = forumRepository;
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
                post.CreateComment(username, commentDto.Content, appUserId);

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
            }
            catch (Exception)
            {
                await _unitOfWork.Rollback();
                throw;
            }
        }
    }
}
