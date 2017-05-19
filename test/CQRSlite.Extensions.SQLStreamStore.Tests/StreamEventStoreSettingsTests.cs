using System;
using CQRSlite.Events;
using CQRSlite.Extensions.SQLStreamStore;
using NSubstitute;
using SqlStreamStore;
using Xunit;

namespace CQRSlite.Extensions.SQLStreamStore
{
    public class StreamEventStoreSettingsTests
    {
        [Fact]
        public void Construction_requires_a_stream_store_factory_delegate()
        {
            var serializationFacade = Substitute.For<ISerializationFacade>();
            var eventPublisher = Substitute.For<IEventPublisher>();
            Assert.Throws<ArgumentNullException>("streamStore",
                () => new StreamEventStoreSettings(
                        null, 
                        serializationFacade, 
                        eventPublisher, 
                        DefaultStreamIdExtractor.Instance,
                        DefaultEventMetadataExtractor.Instance));
        }

        [Fact]
        public void Construction_requires_a_serialization_facade()
        {
            var streamStore = Substitute.For<IStreamStore>();
            var eventPublisher = Substitute.For<IEventPublisher>();
            Assert.Throws<ArgumentNullException>("serializationFacade",
                () => new StreamEventStoreSettings(
                        streamStore, 
                        null, 
                        eventPublisher, 
                        DefaultStreamIdExtractor.Instance,
                        DefaultEventMetadataExtractor.Instance));
        }


        [Fact]
        public void Construction_requires_an_event_publusher()
        {
            var streamStore = Substitute.For<IStreamStore>();
            var serializationFacade = Substitute.For<ISerializationFacade>();
            Assert.Throws<ArgumentNullException>("eventPublisher",
                () => new StreamEventStoreSettings(
                        streamStore, 
                        serializationFacade, 
                        null, 
                        DefaultStreamIdExtractor.Instance,
                        DefaultEventMetadataExtractor.Instance));
        }

        [Fact]
        public void Construction_requires_a_stream_id_extractors()
        {
            var streamStore = Substitute.For<IStreamStore>();
            var serializationFacade = Substitute.For<ISerializationFacade>();
            var eventPublisher = Substitute.For<IEventPublisher>();
            Assert.Throws<ArgumentNullException>("streamIdExtractor",
                () => new StreamEventStoreSettings(
                    streamStore, 
                    serializationFacade, 
                    eventPublisher, 
                    null,
                    DefaultEventMetadataExtractor.Instance));
        }

        [Fact]
        public void Construction_requires_a_event_stream_metadata_extractors()
        {
            var streamStore = Substitute.For<IStreamStore>();
            var serializationFacade = Substitute.For<ISerializationFacade>();
            var eventPublisher = Substitute.For<IEventPublisher>();
            Assert.Throws<ArgumentNullException>("eventMetadataExtractor",
                () => new StreamEventStoreSettings(
                    streamStore,
                    serializationFacade,
                    eventPublisher,
                    DefaultStreamIdExtractor.Instance,
                    null));
        }
    }
}
