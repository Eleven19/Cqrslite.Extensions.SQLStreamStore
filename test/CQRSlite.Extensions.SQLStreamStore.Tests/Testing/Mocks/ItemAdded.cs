using System;
using CQRSlite.Events;

namespace CQRSlite.Extensions.SQLStreamStore.Testing.Mocks
{
    public class ItemAdded : IEvent, IEquatable<ItemAdded>
    {
        public ItemAdded(Guid id, string sku, string description, int quantity)
        {
            Id = id;
            Sku = sku;
            Description = description;
            Quantity = quantity;
        }
        public Guid Id { get; set; }
        public string Sku { get; }
        public string Description { get; }
        public int Quantity { get; }
        public int Version { get; set; }
        public DateTimeOffset TimeStamp { get; set; }
        
        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(Sku)}: {Sku}, {nameof(Description)}: {Description}, {nameof(Quantity)}: {Quantity}, {nameof(Version)}: {Version}, {nameof(TimeStamp)}: {TimeStamp}";
        }

        public bool Equals(ItemAdded other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id.Equals(other.Id) && string.Equals(Sku, other.Sku, StringComparison.OrdinalIgnoreCase) && string.Equals(Description, other.Description, StringComparison.OrdinalIgnoreCase) && Quantity == other.Quantity && Version == other.Version && TimeStamp.Equals(other.TimeStamp);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ItemAdded) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Id.GetHashCode();
                hashCode = (hashCode * 397) ^ (Sku != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(Sku) : 0);
                hashCode = (hashCode * 397) ^ (Description != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(Description) : 0);
                hashCode = (hashCode * 397) ^ Quantity;
                hashCode = (hashCode * 397) ^ Version;
                hashCode = (hashCode * 397) ^ TimeStamp.GetHashCode();
                return hashCode;
            }
        }

        public static bool operator ==(ItemAdded left, ItemAdded right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ItemAdded left, ItemAdded right)
        {
            return !Equals(left, right);
        }
    }
}