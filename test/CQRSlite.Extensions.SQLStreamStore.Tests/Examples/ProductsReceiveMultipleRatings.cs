using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CQRSlite.Domain;
using CQRSlite.Events;
using CQRSlite.Extensions.SQLStreamStore.Testing;
using SqlStreamStore;
using Xunit.Abstractions;

namespace CQRSlite.Extensions.SQLStreamStore.Examples
{
    public class ProductsReceiveMultipleRatings : ScenarioFor<StreamEventStore>
    {
        public IList<Guid> ProductIds { get;} = new List<Guid>();
        public Random Randomizer { get; } = new Random();
        public IRepository Repository { get; private set; }

        public ProductsReceiveMultipleRatings(ITestOutputHelper output):base(output)
        {
            
        }
        public void Given_an_event_store()
        {
            var settings = new StreamEventStoreSettingsBuilder()
                .UseStreamStore(new InMemoryStreamStore())
                .UseInProcessBus()
                .Build();

            SUT = new StreamEventStore(settings);
        }

        public void And_given_a_repository()
        {
            Repository = new Repository(SUT);
        }

        public async Task And_given_a_set_of_preexisting_products()
        {
            var session = new Session(new Repository(SUT));

            var products = new[]
            {
                new Product(Guid.NewGuid(), "VR Headset"),
                new Product(Guid.NewGuid(), "Bluetooth Speaker"),
            } ;

            foreach (var product in products)
            {
                await session.Add(product);
                ProductIds.Add(product.Id);
            }

            await session.Commit();
        }

        public async Task When_we_add_a_first_set_of_product_ratings()
        {
            
            foreach (var productId in ProductIds)
            {
                //var session = new Session(Repository);
                var product = await Repository.Get<Product>(productId);

                var numReviews = Randomizer.Next(1, 6);
                for (int idx = 0; idx < numReviews; idx++)
                {
                    var rating = (CustomerRating) Randomizer.Next(1, 6);
                    product.RateProduct(rating);
                }
                await Repository.Save(product);
                //await session.Commit();
            }
            
        }
    }
}
