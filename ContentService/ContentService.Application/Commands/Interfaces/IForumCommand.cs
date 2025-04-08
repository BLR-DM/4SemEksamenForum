using ContentService.Application.Commands.CommandDto.ForumDto;
using ContentService.Application.Commands.CommandDto.PostDto;

namespace ContentService.Application.Commands.Interfaces
{
    public interface IForumCommand
    {
        // Forum
        Task CreateForumAsync(CreateForumDto forumDto, string appUserId);
        Task UpdateForumAsync(UpdateForumDto forumDto, string appUserId, int forumId);
        Task HandleApprovalAsync(PublishForumDto forumDto);
        Task HandlePublishAsync(PublishForumDto forumDto);
        Task DeleteForumAsync(DeleteForumDto forumDto, int forumId);

        // Post
        Task CreatePostAsync(CreatePostDto postDto, string username, string appUserId, int forumId);
        Task UpdatePostAsync(UpdatePostDto postDto, string appUserId, int forumId, int postId);
        Task DeletePostAsync(DeletePostDto postDto, string appUserId, int forumId, int postId);
    }
}