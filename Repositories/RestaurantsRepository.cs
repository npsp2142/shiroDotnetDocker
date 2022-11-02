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

        private const string DefaultSortKey = "creationTime";
        private const int DefaultSortOrder = 1;
        private const int DefaultFoodOrdersPerPage = 20;


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
                await _restaurantsCollection.InsertOneAsync(restaurant);
                var resultRestaurant = await _restaurantsCollection
                    .Find(Builders<Restaurant>.Filter.Eq(r => r.NameEn, restaurant.NameEn))
                    .FirstOrDefaultAsync();
                Console.WriteLine("Added new restaurant --- " + resultRestaurant.ToJson());
                return new RestaurantResponse(restaurant);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                return new RestaurantResponse(false, exception.ToJson());
            }
        }

        public async Task<Restaurant> GetRestaurantByIdAsync(ObjectId restaurantId, CancellationToken cancellationToken = default)
        {
            var filter = Builders<Restaurant>.Filter.Eq(r => r.Id, restaurantId);
            return await _restaurantsCollection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyList<Restaurant>> GetRestaurantsAsync(
             CancellationToken cancellationToken = default,
             string sortKey = DefaultSortKey,
             int sortOrder = DefaultSortOrder,
             int foodOrdersPerPage = DefaultFoodOrdersPerPage,
             int page = 0
         )
        {
            var skip = foodOrdersPerPage * page;
            var limit = foodOrdersPerPage;

            var sortFilter = new BsonDocument(sortKey, sortOrder);
            var restaurants = await _restaurantsCollection
                .Find(Builders<Restaurant>.Filter.Empty)
                .Limit(limit)
                .Skip(skip)
                .Sort(sortFilter)
                .ToListAsync(cancellationToken);
            Console.WriteLine("Get Restaurant --- " + restaurants.ToJson());
            return restaurants;
        }

        public async Task<long> GetRestaurantsCountAsync()
        {
            return await _restaurantsCollection.CountDocumentsAsync(Builders<Restaurant>.Filter.Empty);
        }

        public async Task<UpdateResult> AddFoodsAsync(List<Food> foods, ObjectId restaurantId)
        {
            try
            {
                List<Food> foodsToAdd = new();
                var filter = Builders<Restaurant>.Filter.Eq(r => r.Id, restaurantId);
                var restaurant = await _restaurantsCollection.Find(filter).FirstOrDefaultAsync();
                foreach (var food in foods)
                {
                    if (!restaurant.Menu.Contains(food))
                        foodsToAdd.Add(food);
                }

                var update = Builders<Restaurant>.Update.AddToSetEach(r => r.Menu, foodsToAdd);
                
                //var res = await _restaurantsCollection.UpdateOneAsync(filter, update);
                return await _restaurantsCollection.UpdateOneAsync(filter, update);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                return null;
            }
        }

        public async Task<UpdateResult> DeleteFoodsAsync(List<string> foodNames, ObjectId restaurantId)
        {
            try
            {
                
                var filter = Builders<Restaurant>.Filter.Eq(r => r.Id, restaurantId);
                var restaurant = await _restaurantsCollection.Find(filter).FirstOrDefaultAsync();
                var foodsToDel = restaurant.Menu.Where(f => foodNames.Contains(f.NameEn)).ToList();

                var update = Builders<Restaurant>.Update.PullAll(r => r.Menu, foodsToDel);

                //var res = await _restaurantsCollection.UpdateOneAsync(filter, update);
                return await _restaurantsCollection.UpdateOneAsync(filter, update);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                return null;
            }
        }
    }
}
