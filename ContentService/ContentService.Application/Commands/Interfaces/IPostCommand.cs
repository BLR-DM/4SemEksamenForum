using ContentService.Application.Commands.CommandDto.CommentDto;

namespace ContentService.Application.Commands.Interfaces
{
    public interface IPostCommand
    {
        Task CreateCommentAsync(CreateCommentDto commentDto, string username, int postId, string appUserId, int forumId);
        Task UpdateCommentAsync(UpdateCommentDto commentDto, string appUserId, int forumId, int postId, int commentId);
        Task DeleteCommentAsync(DeleteCommentDto commentDto, string appUserId, int forumId, int postId, int commentId);
    }
}