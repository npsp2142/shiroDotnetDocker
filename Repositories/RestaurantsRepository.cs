using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using shiroDotnetRestfulDocker.Models.Responses;
using shiroDotnetRestfulDocker.Models;
using MongoDB.Bson;

namespace shiroDotnetRestfulDocker.Repositories
{
    public class RestaurantsRepository
    {
        private readonly IMongoCollection<Restaurant> _restaurantsCollection;

        public RestaurantsRepository(IMongoClient mongoClient, IOptions<OrderJnjDatabaseSettings> settings)
        {
            var camelCaseConvention = new ConventionPack { new CamelCaseElementNameConvention() };
            ConventionRegistry.Register("CamelCase", camelCaseConvention, type => true);

            _restaurantsCollection = mongoClient
                .GetDatabase(settings.Value.DatabaseName)
                .GetCollection<Restaurant>(settings.Value.RestaurantsCollectionName);
        }

        public async Task<RestaurantResponse> AddRestaurantAsync(string name, string description,
           CancellationToken cancellationToken = default)
        {
            try
            {
                var newRestaurant = new Restaurant();
                newRestaurant.Name = name;
                newRestaurant.Description = description;
                await _restaurantsCollection.InsertOneAsync(newRestaurant);
                var resultRestaurant = await _restaurantsCollection
                    .Find(Builders<Restaurant>.Filter.Eq(r => r.Name, name))
                    .FirstOrDefaultAsync();
                Console.WriteLine("Added new restaurant --- " + resultRestaurant.ToJson());
                return new RestaurantResponse();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                return new RestaurantResponse();
            }
        }
    }
}
