using System;
using CQRSlite.Extensions.SQLStreamStore;
using FluentAssertions;
using NSubstitute;
using SqlStreamStore;
using Xunit;

namespace CQRSlite.Extensions.SQLStreamStore
{
    public class StreamEventStoreTests
    {
        public StreamEventStoreTests()
        {
        }

        [Fact]
        public void Construction_requires_non_null_settings()
        {
            Assert.Throws<ArgumentNullException>("settings",
                () => new StreamEventStore(null));
        }

        [Fact]
        public void Constructing_should_set_StreamStore_property()
        {
            var settings = new StreamEventStoreSettingsBuilder()
                .UseStreamStore(Substitute.For<IStreamStore>())
                .UseInProcessBus()
                .Build();

            var sut = new StreamEventStore(settings);

            sut.StreamStore.Should().BeSameAs(settings.StreamStore);
        }
    }
}