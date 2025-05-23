﻿using VoteService.Domain.Entities;

namespace VoteService.Domain.Interfaces;

public interface ICommentVoteRepository
{
    Task<CommentVote?> GetVoteByUserIdAsync(string userId, int commentId);
    Task AddCommentVoteAsync(CommentVote commentVote);
    Task UpdateVoteAsync(CommentVote commentVote);
    Task DeleteCommentVoteAsync(CommentVote commentVote);
}