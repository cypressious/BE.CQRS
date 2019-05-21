using BE.CQRS.Domain.DomainObjects;

namespace BE.CQRS.Domain.Events
{
    public interface IEvent
    {
        EventHeader Headers { get; }

        bool Validate();

        void AssertValidation();

        IEvent LinkTo<T>(string aggregateId) where T : IDomainObject;
    }
}