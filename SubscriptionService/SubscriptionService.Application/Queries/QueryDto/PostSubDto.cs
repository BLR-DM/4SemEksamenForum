using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubscriptionService.Application.Queries.QueryDto
{
    public class PostSubDto
    {
        public string AppUserId { get; set; } = string.Empty;
        public int PostId { get; set; }
        public DateTime SubscribedAt { get; set; }
    }
}
