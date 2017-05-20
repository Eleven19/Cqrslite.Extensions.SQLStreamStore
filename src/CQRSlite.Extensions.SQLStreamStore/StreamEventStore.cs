using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQRSlite.Events;
using SqlStreamStore;
using SqlStreamStore.Streams;

namespace CQRSlite.Extensions.SQLStreamStore
{
    /// <summary>
    /// Provides an <see cref="IEventStore"/> implementation that is backed by an <see cref="IStreamStore"/>.
    /// </summary>
    /// <remarks>See https://github.com/damianh/SqlStreamStore to learn more about <see cref="IStreamStore"/>.</remarks>
    public class StreamEventStore : IEventStore
    {
        public StreamEventStore(StreamEventStoreSettings settings)
        {
            Settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        protected internal StreamEventStoreSettings Settings { get; }
        protected internal IStreamStore StreamStore => Settings.StreamStore;
        public ISerializationFacade SerializationFacade => Settings.SerializationFacade;
        public IEventPublisher EventPublisher => Settings.EventPublisher;

        public async Task Save(IEnumerable<IEvent> events)
        {
            var streamEvents = new Dictionary<string, StreamEventBatch>();
            var streamIds = new HashSet<string>();
            foreach (var @event in events)
            {
                var streamId = GetStreamId(@event);
                streamIds.Add(streamId);
                StreamEventBatch batch;
                if (!streamEvents.TryGetValue(streamId, out batch))
                {
                    //var streamMetadata = await StreamStore
                    //    .GetStreamMetadata(streamId)
                    //    .ConfigureAwait(false);

                    batch = new StreamEventBatch(streamId)
                    {                     
                        Version = @event.Version //streamMetadata.MetadataStreamVersion
                    };                    
                    streamEvents.Add(streamId, batch);
                    
                }

                var metadata = GetEventMetadata(@event);
                object metadataValue;
                Guid messageId = Guid.Empty;
                if (metadata.TryGetValue(MetadataPropertyNames.MessageId, out metadataValue))
                {
                    if (metadataValue is Guid msgId)
                    {
                        messageId = msgId;
                    }                    
                }

                if (messageId == Guid.Empty)
                {
                    messageId = Guid.NewGuid();
                }

                var eventType = @event.GetType();
                var jsonData = SerializationFacade.Serialize(@event, eventType);
                var metadataJson = SerializationFacade.Serialize(metadata, metadata.GetType());
                var newStreamMessage = new NewStreamMessage(messageId, eventType.FullName, jsonData, metadataJson);
                batch.Messages.Add(newStreamMessage);
                batch.Version = Math.Min(batch.Version, @event.Version);
            }

            foreach (var streamId in streamIds)
            {
                var batch = streamEvents[streamId];
                var messages = batch.Messages.ToArray();
                var version = batch.Version <= 0 ? ExpectedVersion.NoStream : batch.Version;
                await StreamStore
                    .AppendToStream(streamId, ExpectedVersion.Any, messages)
                    .ConfigureAwait(false);
            }
        }

        public async Task<IEnumerable<IEvent>> Get(Guid aggregateId, int fromVersion)
        {
            var expectedVersion = fromVersion == -1 ? ExpectedVersion.Any : fromVersion;

            var streamId = aggregateId.ToString();

            var pageSize = 100;
            var startingVersion = StreamVersion.Start; //Math.Max(StreamVersion.Start, expectedVersion);
            int messagesRead = 0;
            var events = new List<IEvent>();
            do
            {
                var page = await StreamStore
                    .ReadStreamForwards(streamId, startingVersion, pageSize)
                    .ConfigureAwait(false);

                messagesRead = page.Messages.Length;
                foreach (var message in page.Messages)
                {
                    var @event = await DeserializeDomainEvent(message).ConfigureAwait(false);
                    events.Add(@event);
                    startingVersion = startingVersion + messagesRead;
                }
            } while (messagesRead >= pageSize);
            return events;
        }

        protected virtual string GetStreamId(IEvent @event) => Settings.StreamIdExtractor(@event);

        protected virtual IDictionary<string, object> GetEventMetadata(IEvent @event) => 
            Settings.EventMetadataExtractor(@event);


        private async Task<IEvent> DeserializeDomainEvent(StreamMessage message)
        {
            var metadata = SerializationFacade.Deserialize<IDictionary<string,object>>(message.JsonMetadata);
            Type messageType = null;

            // Perform case sensitive look up
            foreach (var typeName in GetCandidateMessageTypeNames(metadata))
            {
                messageType = Type.GetType(typeName, false, false);
                if (messageType != null) break;
            }

            if (messageType == null)
            {
                // Perform case insensitive lookup
                foreach (var typeName in GetCandidateMessageTypeNames(metadata))
                {
                    messageType = Type.GetType(typeName, false, true);
                    if (messageType != null) break;
                }
            }

            if (messageType == null)
            {
                throw new TypeResolutionException("Cannot deserialize the event because the event type could not be resolved.");
            }

            var messageJson = await message.GetJsonData().ConfigureAwait(false);
            var deserializedMessage = SerializationFacade.Deserialize(messageJson, messageType);
            return (IEvent)deserializedMessage;

            IEnumerable<string> GetCandidateMessageTypeNames(IDictionary<string, object> messageMetadata)
            {
                if (messageMetadata != null)
                {
                    object value;
                    if (messageMetadata.TryGetValue(MetadataPropertyNames.EventTypeAssemblyQualifiedName, out value))
                    {
                        yield return $"{value}";
                    }

                    if (messageMetadata.TryGetValue(MetadataPropertyNames.EventTypeFullName, out value))
                    {
                        yield return $"{value}";
                    }
                }

                yield return message.Type;
            }
        }

        private class StreamEventBatch
        {            
            public StreamEventBatch(string streamId)
            {
                StreamId = streamId;
            }

            public string StreamId { get; }
            public int Version { get; set; }
            public IList<NewStreamMessage> Messages { get; } = new List<NewStreamMessage>();            
        }
    }
}
