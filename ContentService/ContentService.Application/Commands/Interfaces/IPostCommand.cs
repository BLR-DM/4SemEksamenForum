﻿using ContentService.Application.Commands.CommandDto.CommentDto;
using ContentService.Application.Commands.CommandDto.PostDto;

namespace ContentService.Application.Commands.Interfaces
{
    public interface IPostCommand
    {
        Task CreateCommentAsync(CreateCommentDto commentDto, string username, int postId, string appUserId, int forumId);
        Task UpdateCommentAsync(UpdateCommentDto commentDto, string appUserId, int forumId, int postId, int commentId);
        Task DeleteCommentAsync(string appUserId, int forumId, int postId, int commentId);
        Task HandleCommentApprovalAsync(PublishCommentDto publishCommentDto);
        Task HandleCommentRejectionAsync(RejectCommentDto rejectCommentDto);
        Task DeleteCommentModAsync(int forumId, int postId, int commentId);
    }
}