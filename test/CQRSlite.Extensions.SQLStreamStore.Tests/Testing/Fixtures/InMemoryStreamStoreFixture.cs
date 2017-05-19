using System;
using System.Collections.Generic;
using System.Text;
using SqlStreamStore;
using SqlStreamStore.Infrastructure;

namespace CQRSlite.Extensions.SQLStreamStore.Testing.Fixtures
{
    public class InMemoryStreamStoreFixture
    {
        private readonly Lazy<InMemoryStreamStore> _streamStoreAccessor;
        public InMemoryStreamStoreFixture()
        {
            _streamStoreAccessor = new Lazy<InMemoryStreamStore>(() => new InMemoryStreamStore());
        }

        public InMemoryStreamStore StreamStore => _streamStoreAccessor.Value;

        public InMemoryStreamStore GetStreamStore(GetUtcNow getUtcNow = null) =>
            new InMemoryStreamStore(getUtcNow);
    }
}
