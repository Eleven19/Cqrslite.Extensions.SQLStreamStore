using System;
using CQRSlite.Events;
using SqlStreamStore;

namespace CQRSlite.Extensions.SQLStreamStore
{
    /// <summary>
    /// Provides settings for use in constructing a <see cref="StreamEventStore"/>.
    /// </summary>
    public class StreamEventStoreSettings
    {
        public StreamEventStoreSettings(
            IStreamStore streamStore, 
            ISerializationFacade serializationFacade, 
            IEventPublisher eventPublisher,
            StreamIdExtractor streamIdExtractor,
            EventMetadataExtractor eventMetadataExtractor)
        {
            StreamStore = streamStore ?? throw new ArgumentNullException(nameof(streamStore));
            SerializationFacade = serializationFacade ?? throw new ArgumentNullException(nameof(serializationFacade));
            EventPublisher = eventPublisher ?? throw new ArgumentNullException(nameof(eventPublisher));
            StreamIdExtractor = streamIdExtractor ?? throw new ArgumentNullException(nameof(streamIdExtractor));
            EventMetadataExtractor = eventMetadataExtractor ?? throw new ArgumentNullException(nameof(eventMetadataExtractor));
        }

        /// <summary>
        /// Gets a factory used for constucting an <see cref="IStreamStore"/> instance.
        /// </summary>
        public IStreamStore StreamStore { get; }

        /// <summary>
        /// Gets a serialization facade used to perform serializations.
        /// </summary>
        public ISerializationFacade SerializationFacade { get; }

        /// <summary>
        /// Gets the <see cref="IEventPublisher"/> used to publish events.
        /// </summary>
        public IEventPublisher EventPublisher { get; }

        /// <summary>
        /// Gets an extractor used to extract the stream id of an event.
        /// </summary>
        public StreamIdExtractor StreamIdExtractor { get; }

        /// <summary>
        /// Gets an instance of a metadata extract which can be used to extract stream metadata from an event.
        /// </summary>
        public EventMetadataExtractor EventMetadataExtractor { get; }
    }
}
