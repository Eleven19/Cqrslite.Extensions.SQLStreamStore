using CQRSlite.Extensions.SQLStreamStore;
using CQRSlite.Extensions.SQLStreamStore.Testing;
using Xunit.Abstractions;

namespace CQRSlite.Extensions.SQLStreamStore
{
    public class When_constructing_StreamEventStoreSettings : ScenarioFor<StreamEventStoreSettings>
    {

        public When_constructing_StreamEventStoreSettings(ITestOutputHelper output) : base(output)
        {
        }
    }
 
}