﻿namespace ContentService.Application.EventDto
{
    public record PostEventDtos(int ForumId, int PostId);
    public record PostPublishedDto(string UserId, int ForumId, int PostId);
    public record PostRejectedDto(string UserId, int ForumId, int PostId); // Reason??
    public record PostSubmittedDto(string ContentId, string Content);
}