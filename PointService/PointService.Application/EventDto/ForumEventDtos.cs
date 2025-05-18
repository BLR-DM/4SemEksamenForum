using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PointService.Application.EventDto
{
    public record ForumPublishedDto(string UserId, int ForumId);
    public record ForumDeletedDto(string UserId, int ForumId);
    public record FailedToAddPointsOnForumPublishedDto(string UserId, int ForumId);
}
