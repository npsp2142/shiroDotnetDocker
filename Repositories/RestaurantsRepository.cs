using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using shiroDotnetRestfulDocker.Models;
using shiroDotnetRestfulDocker.Models.Responses;

namespace shiroDotnetRestfulDocker.Repositories
{
    public class RestaurantsRepository
    {
        private readonly IMongoCollection<Restaurant> _restaurantsCollection;
        private readonly IConfiguration _configuration;


        // configuration setting
        // https://learn.microsoft.com/en-us/aspnet/core/fundamentals/configuration/options?view=aspnetcore-6.0
        public RestaurantsRepository(IMongoClient mongoClient, IConfiguration configuration)
        {
            var camelCaseConvention = new ConventionPack { new CamelCaseElementNameConvention() };
            ConventionRegistry.Register("CamelCase", camelCaseConvention, type => true);
            _configuration = configuration;
            var settings = new OrderJnjDatabaseSettings();
            _configuration.GetSection(OrderJnjDatabaseSettings.OrderJnjDatabase).Bind(settings);
            _restaurantsCollection = mongoClient
                .GetDatabase(settings.DatabaseName)
                .GetCollection<Restaurant>(settings.RestaurantsCollectionName);
        }

        public async Task<RestaurantResponse> AddRestaurantAsync(Restaurant restaurant,
           CancellationToken cancellationToken = default)
        {
            try
            {
                //var newRestaurant = new Restaurant();
                //newRestaurant.Name = name;
                //newRestaurant.Description = description;
                await _restaurantsCollection.InsertOneAsync(restaurant);
                var resultRestaurant = await _restaurantsCollection
                    .Find(Builders<Restaurant>.Filter.Eq(r => r.NameEnglish, restaurant.NameEnglish))
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
