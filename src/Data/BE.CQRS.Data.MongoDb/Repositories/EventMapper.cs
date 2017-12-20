using System;
using System.Collections.Generic;
using System.Reflection;
using BE.CQRS.Data.MongoDb.Commits;
using BE.CQRS.Domain.DataProtection;
using BE.CQRS.Domain.Events;
using BE.CQRS.Domain.Serialization;
using BE.FluentGuard;

namespace BE.CQRS.Data.MongoDb.Repositories
{
    public sealed class EventMapper
    {
        private readonly IEventSerializer serializer;

        public EventMapper(IEventSerializer serializer)
        {
            Precondition.For(serializer, nameof(serializer)).NotNull("EventSerializer must be set !");
            this.serializer = serializer;
        }

        public EventCommit ToCommit(string domainobjectId, Type domainObjectType, long originVersion, long commitVersion,
            IList<IEvent> events, IEventDataProtector protector) // TODO THIS IS JUST UGLY !!
        {
            Dictionary<string, EventDto> items = MapEvents(domainobjectId, events, protector);

            var commit = new EventCommit
            {
                AggregateId = domainobjectId,
                AggregateType = domainObjectType.FullName,
                AggregateTypeShort = domainObjectType.Name,
                AggregatePackage = domainObjectType.GetTypeInfo().Assembly.GetName().Name,
                Ordinal = Timestamp.FromNow(),
                Timestamp = DateTime.UtcNow,
                VersionEvents = originVersion + events.Count,
                VersionCommit = commitVersion,
                Events = items
            };

            return commit;
        }

        public IEnumerable<IEvent> ExtractEvents(EventCommit commit, IEventDataProtector protector)
        {
            foreach (KeyValuePair<string, EventDto> @event in commit.Events)
            {
                @event.Value.Headers.Add(EventHeaderKeys.AggregateId, commit.AggregateId);
                @event.Value.Headers.Add(EventHeaderKeys.CommitId, commit.Ordinal.ToString());

                string body = @event.Value.Body;

                body = UnprotectBody(protector, @event, body);

                yield return serializer.DeserializeEvent(@event.Value.Headers, body);
            }
        }

        private static string UnprotectBody(IEventDataProtector protector, KeyValuePair<string, EventDto> @event, string body)
        {
            if (@event.Value.Headers.ContainsKey(EventHeaderKeys.Protected))
            {
                if (bool.Parse(@event.Value.Headers[EventHeaderKeys.Protected]))
                {
                    if (protector == null)
                    {
                        throw new InvalidOperationException("Expected a protector but non was set!");
                    }
                    body = protector.Unprotect(body);
                }
            }
            return body;
        }

        private Dictionary<string, EventDto> MapEvents(string domainObjectId, IList<IEvent> events, IEventDataProtector protector)
        {
            var items = new Dictionary<string, EventDto>(events.Count);

            for (var i = 0; i < events.Count; i++)
            {
                IEvent @event = events[i];
                if (!domainObjectId.Equals(@event.Headers.AggregateId))
                {
                    throw new InvalidOperationException("Domainobject id did not match!");
                }

                string content = serializer.SerializeEvent(@event);

                if (protector != null)
                {
                    content = protector.Protect(content);
                    @event.Headers.SetProtected();
                }
                var dto = new EventDto
                {
                    Body = content,
                    Headers = @event.Headers.ToDictionary(),
                    Id = @event.Headers.GetString(EventHeaderKeys.EventId)
                };

                dto.Headers.Remove(EventHeaderKeys.AggregateId);
                items.Add(i.ToString(), dto);
            }
            return items;
        }
    }
}