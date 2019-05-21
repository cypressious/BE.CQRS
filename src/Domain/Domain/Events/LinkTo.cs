using System;

namespace BE.CQRS.Domain.Events
{
    public sealed class LinkTo
    {
        public string AggregateId { get;  }

        public string AggregateTypFullName { get;  }
 
        public LinkTo(Type type, string id)
        {
            AggregateId = id;
            AggregateTypFullName = type.FullName;
        }
    }
}