using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubscriptionService.Domain.Exceptions
{
    public class AlreadySubscribedException : Exception
    {
        public AlreadySubscribedException(string message) : base(message) { }
    }
}
