using System;
using CQRSlite.Bus;
using SqlStreamStore;
using CQRSlite.Events;
using CQRSlite.Extensions.SQLStreamStore.Internals;

namespace CQRSlite.Extensions.SQLStreamStore
{
    /// <summary>
    /// Provides a builder for <see cref="StreamEventStoreSettings"/>.
    /// </summary>
    public class StreamEventStoreSettingsBuilder : IFluentInterface
    {
        private IStreamStore _streamStore;
        private ISerializationFacade _serializationFacade;
        private IEventPublisher _eventPublisher;
        private StreamIdExtractor _streamIdExtractor;
        private EventMetadataExtractor _eventMetadataExtractor;

        /// <summary>
        /// Sets the provided <see cref="IStreamStore"/> instance as the underlying <see cref="IStreamStore"/> used when constructing a <see cref="StreamEventStore"/> instance.
        /// </summary>
        /// <param name="streamStore">The <see cref="IStreamStore"/> instance</param>
        /// <returns>The current builder instance for method chaining purposes.</returns>
        /// <exception cref="ArgumentNullException">if <paramref name="streamStore"/> is null</exception>
        public StreamEventStoreSettingsBuilder UseStreamStore(IStreamStore streamStore)
        {
            _streamStore = streamStore ?? throw new ArgumentNullException(nameof(streamStore));
            return this;
        }

        /// <summary>
        /// Sets the <see cref="ISerializationFacade"/> used for performing serialization.
        /// </summary>
        /// <param name="serializationFacade"></param>
        /// <returns></returns>
        public StreamEventStoreSettingsBuilder UseSerializationFacade(ISerializationFacade serializationFacade)
        {
            _serializationFacade = serializationFacade ?? throw new ArgumentNullException(nameof(serializationFacade));
            return this;
        }


        /// <summary>
        /// Sets the <see cref="IEventPublisher"/> used for publishing events.
        /// </summary>
        /// <param name="eventPublisher"></param>
        /// <returns></returns>
        public StreamEventStoreSettingsBuilder UseEventPublisher(IEventPublisher eventPublisher)
        {
            _eventPublisher = eventPublisher ?? throw new ArgumentNullException(nameof(eventPublisher));
            return this;
        }

        public StreamEventStoreSettingsBuilder UseInProcessBus(InProcessBus inprocessBus = null)
        {
            _eventPublisher = inprocessBus ?? new InProcessBus();
            return this;
        }

        public StreamEventStoreSettingsBuilder UseStreamIdExtractor(StreamIdExtractor extractor)
        {
            _streamIdExtractor = extractor ?? throw new ArgumentNullException(nameof(extractor));
            return this;
        }

        public StreamEventStoreSettingsBuilder UseDefaultStreamIdExtractor()
        {
            _streamIdExtractor = DefaultStreamIdExtractor.Instance;
            return this;
        }

        public StreamEventStoreSettingsBuilder UseEventStreamMetadataExtractor(EventMetadataExtractor extractor)
        {
            _eventMetadataExtractor = extractor;
            return this;
        }

        public StreamEventStoreSettingsBuilder UseDefaultStreamMetadataExtractor()
        {
            _eventMetadataExtractor = DefaultEventMetadataExtractor.Instance;
            return this;
        }

        /// <summary>
        /// Builds a new instance of <see cref="StreamEventStoreSettings"/>.
        /// </summary>
        /// <returns>a new instance of <see cref="StreamEventStoreSettings"/></returns>
        public StreamEventStoreSettings Build()
        {
            if (_streamStore == null)
            {
                throw new InvalidOperationException("A stream store instance must be provided.");
            }

            if (_eventPublisher == null)
            {
                throw new InvalidOperationException("An IEventPublisher instance must be provided.");
            }

            var serializationFacade = _serializationFacade ?? JsonDotNetSerializationFacade.Create();
            var eventPublisher = _eventPublisher;
            
            return new StreamEventStoreSettings(
                _streamStore, 
                serializationFacade, 
                eventPublisher,
                _streamIdExtractor ?? DefaultStreamIdExtractor.Instance,
                _eventMetadataExtractor ?? DefaultEventMetadataExtractor.Instance);
        }
    }
}