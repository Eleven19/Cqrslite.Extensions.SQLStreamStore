using System;
using CQRSlite.Commands;
using CQRSlite.Domain;
using CQRSlite.Events;

namespace CQRSlite.Extensions.SQLStreamStore.Examples
{
    public class Product: AggregateRoot
    {
        private long _numberOfRatings = 0;
        private long _5StarRatings;
        private long _4StarRatings;
        private long _3StarRatings;
        private long _2StarRatings;
        private long _1StarRatings;

        private Product() {}
        public Product(Guid id, string name)
        {
            Id = id;
            ApplyChange(new ProductCreated(id, name));
        }

        public void RateProduct(CustomerRating rating)
        {
            if (rating == CustomerRating.None)
            {
                throw new ArgumentOutOfRangeException(nameof(rating), "The product must be rated either: 5, 4, 3, 2, or 1 star(s).");
            }
            ApplyChange(new ProductRated(Id, rating));
        }

        public double? GetAverageRating()
        {
            var numRatings = _5StarRatings + _4StarRatings + _3StarRatings + _2StarRatings + _1StarRatings;
            if (numRatings == 0) return null;
            var total = (_5StarRatings * 5.0) + (_4StarRatings*4.0)+(_3StarRatings * 3.0)+(_2StarRatings*2.0)+(_1StarRatings);
            return total/numRatings;
        }

        private void Apply(ProductCreated e)
        {            
        }

        private void Apply(ProductRated e)
        {            
            switch (e.Rating)
            {
                case CustomerRating.None:
                    break;
                case CustomerRating.OneStar:
                    _1StarRatings++;
                    _numberOfRatings++;
                    break;
                case CustomerRating.TwoStars:
                    _2StarRatings++;
                    _numberOfRatings++;
                    break;
                case CustomerRating.ThreeStars:
                    _3StarRatings++;
                    _numberOfRatings++;
                    break;
                case CustomerRating.FourStars:
                    _4StarRatings++;
                    _numberOfRatings++;
                    break;
                case CustomerRating.FiveStars:
                    _5StarRatings++;
                    _numberOfRatings++;
                    break;
                default:
                    break;
            }
        }
    }

    public enum CustomerRating
    {
        None = 0,
        OneStar = 1,
        TwoStars = 2,
        ThreeStars = 3,
        FourStars = 4,
        FiveStars = 5
    }

    public class CreateProduct : ICommand
    {
        public CreateProduct(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public int ExpectedVersion { get; set; }
        public Guid Id { get; }
        public string Name { get; }
    }

    public class RateProduct : ICommand
    {        
        public RateProduct(Guid id, CustomerRating rating)
        {
            Id = id;
            Rating = rating;
        }

        public Guid Id { get; }
        public CustomerRating Rating { get; }
        public int ExpectedVersion { get; set; }
    }

    public class ProductCreated : IEvent
    {
        public ProductCreated(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public string Name { get; }

        public Guid Id { get ; set ; }
        public int Version { get; set; }
        public DateTimeOffset TimeStamp { get; set; }        
    }

    public class ProductRated : IEvent
    {
        public ProductRated(Guid id, CustomerRating rating)
        {
            Id = id;
            Rating = rating;
        }

        public CustomerRating Rating { get; set; }

        public Guid Id { get; set; }
        public int Version { get; set; }
        public DateTimeOffset TimeStamp { get; set; }
    }

}
