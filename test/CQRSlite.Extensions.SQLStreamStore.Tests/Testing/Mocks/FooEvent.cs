using System;
using System.Collections.Generic;
using System.Text;
using CQRSlite.Events;

namespace CQRSlite.Extensions.SQLStreamStore.Testing.Mocks
{
    public class FooEvent : IEvent
    {
        public FooEvent(Guid id, string status, int version = 0, DateTimeOffset? timeStamp = null)
        {
            Id = id;
            Status = status;
        }

        public Guid Id { get; set; }
        public string Status { get; }

        public int Version { get; set; }
        public DateTimeOffset TimeStamp { get; set; }

    }
}
