using System;

namespace BE.CQRS.Domain.Events
{
    public sealed class LinkTo
    {
        public string AggregateId { get; set; }

        public string AggregateTypFullName { get; set; }
        public string AssemblyQualifiedName { get; set; }

        public LinkTo()
        {
        }

        public LinkTo(Type type, string id)
        {
            AggregateId = id;
            AssemblyQualifiedName = type.AssemblyQualifiedName;
            AggregateTypFullName = type.FullName;
        }
    }
}