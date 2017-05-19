using System.Collections.Generic;
using CQRSlite.Events;

namespace CQRSlite.Extensions.SQLStreamStore
{
    /// <summary>
    /// Extracts metadata from an event for use in a stream.
    /// </summary>
    /// <param name="event"></param>
    /// <returns></returns>
    public delegate IDictionary<string,object> EventMetadataExtractor(IEvent @event);

    public static class DefaultEventMetadataExtractor
    {
        public static readonly EventMetadataExtractor Instance = e =>
        {
            var eventType = e.GetType();
            return new Dictionary<string, object>
            {
                { MetadataPropertyNames.ContractName, eventType.FullName },
                { MetadataPropertyNames.EventTypeFullName, eventType.FullName },
                { MetadataPropertyNames.EventTypeAssemblyQualifiedName, eventType.AssemblyQualifiedName },
            };
        };
    }
}