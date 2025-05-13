using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PointService.Application.EventDto
{
    public record UserSubscribedToForumEventDto(string UserId, int SubscriptionId, int ForumId);
    public record UserUnSubscribedFromForumEventDto(string UserId, int SubscriptionId, int ForumId);
    public record UserSubscribedToPostEventDto(string UserId, int SubscriptionId, int PostId);
    public record UserUnSubscribedFromPostEventDto(string UserId, int SubscriptionId, int PostId);
}
