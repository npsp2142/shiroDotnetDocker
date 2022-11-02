using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using shiroDotnetRestfulDocker.Models;
using shiroDotnetRestfulDocker.Models.Responses;

namespace shiroDotnetRestfulDocker.Repositories
{
    public class FoodOrdersRepository
    {
        private readonly IMongoCollection<FoodOrder> _foodOrdersCollection;

        private const string DefaultSortKey = "creationTime";
        private const int DefaultSortOrder = 1;
        private const int DefaultFoodOrdersPerPage = 20;
        private readonly IConfiguration _configuration;

        public FoodOrdersRepository(IMongoClient mongoClient, IConfiguration configuration)
        {
            var camelCaseConvention = new ConventionPack { new CamelCaseElementNameConvention() };
            ConventionRegistry.Register("CamelCase", camelCaseConvention, type => true);
            _configuration = configuration;
            var settings = new OrderJnjDatabaseSettings();
            _configuration.GetSection(OrderJnjDatabaseSettings.OrderJnjDatabase).Bind(settings);
            _foodOrdersCollection = mongoClient
                .GetDatabase(settings.DatabaseName)
                .GetCollection<FoodOrder>(settings.FoodOrdersCollectionName);
        }


        public async Task<FoodOrderResponse> AddFoodOrderAsync(FoodOrder foodOrder,
           CancellationToken cancellationToken = default)
        {
            try
            {
                await _foodOrdersCollection.InsertOneAsync(foodOrder);
                var resultFoodOrder = await _foodOrdersCollection
                    .Find(Builders<FoodOrder>.Filter.Eq(r => r.NameEn, foodOrder.NameEn))
                    .FirstOrDefaultAsync();
                Console.WriteLine("Added new FoodOrder --- " + resultFoodOrder.ToJson());
                return new FoodOrderResponse(resultFoodOrder);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                return new FoodOrderResponse(false,"Something went wrong");
            }
        }

        public async Task<IReadOnlyList<FoodOrder>> GetFoodOrdersByRestaurant(
                ObjectId restaurantId,
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
            var foodOrders = await _foodOrdersCollection
                .Find(Builders<FoodOrder>.Filter.Eq(e => e.Id, restaurantId))
                .Limit(limit)
                .Skip(skip)
                .Sort(sortFilter)
                .ToListAsync(cancellationToken);
            Console.WriteLine("Get FoodOrders By Restaurant-- - " + foodOrders.ToJson());
            return foodOrders;
        }
    }
}
