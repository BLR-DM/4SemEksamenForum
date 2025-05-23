﻿using ContentService.Application.Queries.QueryDto;

namespace ContentService.Application.Queries
{
    public interface IForumQuery
    {
        Task<ForumDto> GetForumAsync(int forumId);
        Task<ForumDto> GetForumByNameWithPostsAsync(string forumName);
        Task<List<ForumDto>> GetForumsWithPostsAsync();
        Task<IEnumerable<ForumDto>> GetForumsAsync();
        Task<ForumDto> GetForumWithPostsAsync(int forumId);
        Task<ForumDto> GetForumWithSinglePostAsync(int forumId, int postId);
        Task<ForumDto> GetForumByNameWithSinglePostAsync(string forumName, int postId);
        //Task<ForumDto> GetForumWithPostsByDateRange(int forumId, string appUserId, string role, DateOnly fromDate, DateOnly toDate, int reqUpvotes);
    }
}