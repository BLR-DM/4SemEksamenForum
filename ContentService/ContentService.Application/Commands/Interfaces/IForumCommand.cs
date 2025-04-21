using ContentService.Application.Commands.CommandDto.ForumDto;
using ContentService.Application.Commands.CommandDto.PostDto;

namespace ContentService.Application.Commands.Interfaces
{
    public interface IForumCommand
    {
        // Forum
        Task CreateForumAsync(CreateForumDto forumDto, string appUserId);
        Task UpdateForumAsync(UpdateForumDto forumDto, string appUserId, int forumId);
        Task HandleForumApprovalAsync(PublishForumDto forumDto);
        Task HandleForumRejectedAsync(RejectForumDto forumDto);
        Task HandlePostApprovalAsync(PublishPostDto postDto);
        Task DeleteForumAsync(DeleteForumDto forumDto, int forumId);

        // Post
        Task CreatePostAsync(CreatePostDto postDto, string username, string appUserId, int forumId);
        Task UpdatePostAsync(UpdatePostDto postDto, string appUserId, int forumId, int postId);
        Task HandlePostRejectionAsync(RejectPostDto postDto);
        Task DeletePostAsync(DeletePostDto postDto, string appUserId, int forumId, int postId);
        
    }
}