﻿using BE.CQRS.Domain.DomainObjects;

namespace BE.CQRS.Data.GetEventStore.IntegrationTests
{
    public sealed class SampleDomainObject : DomainObjectBase
    {
        public SampleDomainObject(string id) : base(id)
        {
        }

        public void Add()
        {
            RaiseEvent<SimpleEvent>(i => i.Value = "aa");
        }
    }
}