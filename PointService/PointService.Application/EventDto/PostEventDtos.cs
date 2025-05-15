using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PointService.Application.EventDto
{
    public record PostPublishedDto(string UserId, int ForumId, int PostId);
    public record PostDeletedDto(string UserId, int ForumId, int PostId);

    public record FailedToAddPointsOnForumPublishedDto(string UserId, int ForumId);
    public record FailedToAddPointsOnPostPublishedDto(string UserId, int ForumId, int PostId);
    
}
