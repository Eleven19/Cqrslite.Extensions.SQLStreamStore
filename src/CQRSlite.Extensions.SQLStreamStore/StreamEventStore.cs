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
                    var streamMetadata = await StreamStore
                        .GetStreamMetadata(streamId)
                        .ConfigureAwait(false);

                    batch = new StreamEventBatch(streamId)
                    {
                        MetadataResult =  streamMetadata,
                        Version = streamMetadata.MetadataStreamVersion
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
            }

            foreach (var streamId in streamIds)
            {
                var batch = streamEvents[streamId];
                var messages = batch.Messages.ToArray();
                await StreamStore
                    .AppendToStream(streamId, batch.Version, messages)
                    .ConfigureAwait(false);
            }
        }

        public Task<IEnumerable<IEvent>> Get(Guid aggregateId, int fromVersion)
        {
            throw new NotImplementedException();
        }

        protected virtual string GetStreamId(IEvent @event) => Settings.StreamIdExtractor(@event);

        protected virtual IDictionary<string, object> GetEventMetadata(IEvent @event) => 
            Settings.EventMetadataExtractor(@event);


        private class StreamEventBatch
        {            
            public StreamEventBatch(string streamId)
            {
                StreamId = streamId;
            }

            public string StreamId { get; }
            public int Version { get; set; }
            public StreamMetadataResult MetadataResult { get; set; }
            public IList<NewStreamMessage> Messages { get; } = new List<NewStreamMessage>();            
        }
    }
}
