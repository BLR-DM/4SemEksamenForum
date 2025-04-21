using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubscriptionService.Application.EventDto
{
    public record UserSubscribedToForumEventDto(string UserId, int SubscriptionId, int ForumId);

    public record UserSubscribedToPostEventDto(string UserId, int SubscriptionId, int PostId);

    public record UserUnSubscribedToForumEventDto(string UserId, int SubscriptionId);

    public record UserUnSubscribedToPostEventDto(string UserId, int SubscriptionId);
}
