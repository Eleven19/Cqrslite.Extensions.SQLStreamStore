using System.Diagnostics.Tracing;
using CQRSlite.Events;

namespace CQRSlite.Extensions.SQLStreamStore
{
    /// <summary>
    /// Provides a delegate which can be used to extract the stream id from an <see cref="IEvent"/>
    /// </summary>
    /// <param name="event"></param>
    /// <returns></returns>
    public delegate string StreamIdExtractor(IEvent @event);


    public static class DefaultStreamIdExtractor
    {
        public static readonly StreamIdExtractor Instance = e => e.Id.ToString();
    }
}