﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubscriptionService.Application.Queries.QueryDto
{
    public class ForumSubDto
    {
        public string AppUserId { get; set; } = string.Empty;
        public int ForumId { get; set; }
        public DateTimeOffset SubscribedAt { get; set; }
    }
}
