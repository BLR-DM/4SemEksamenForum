﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PointService.Application.EventDto
{
    public record CommentPublishedDto(string UserId, int ForumId, int PostId, int CommentId);
    public record CommentDeletedDto(string UserId, int ForumId, int PostId, int CommentId);
    public record FailedToAddPointsOnCommentPublishedDto(string UserId, int ForumId, int PostId, int CommentId);
}
