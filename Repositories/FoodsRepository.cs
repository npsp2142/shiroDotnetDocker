using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using shiroDotnetRestfulDocker.Models;
using shiroDotnetRestfulDocker.Models.Responses;

namespace shiroDotnetRestfulDocker.Repositories
{
    public class FoodsRepository
    {
        private readonly IMongoCollection<Food> _foodsCollection;

        public FoodsRepository(IMongoClient mongoClient, IOptions<OrderJnjDatabaseSettings> settings)
        {
            var camelCaseConvention = new ConventionPack { new CamelCaseElementNameConvention() };
            ConventionRegistry.Register("CamelCase", camelCaseConvention, type => true);

            _foodsCollection = mongoClient
                .GetDatabase(settings.Value.DatabaseName)
                .GetCollection<Food>(settings.Value.FoodsCollectionName);
        }

        public async Task<FoodResponse> AddFoodAsync(string nameEnglish, string description,
           CancellationToken cancellationToken = default)
        {
            try
            {
                var newFood = new Food();
                newFood.NameEnglish = nameEnglish;
                newFood.Description = description;
                await _foodsCollection.InsertOneAsync(newFood);
                var resultRestaurant = await _foodsCollection
                    .Find(Builders<Food>.Filter.Eq(r => r.NameEnglish, nameEnglish))
                    .FirstOrDefaultAsync();
                Console.WriteLine("Added new food --- " + resultRestaurant.ToJson());
                return new FoodResponse();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                return new FoodResponse();
            }
        }
    }
}
