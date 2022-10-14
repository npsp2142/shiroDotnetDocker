using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using shiroDotnetRestfulDocker.Models;
using shiroDotnetRestfulDocker.Models.Responses;

namespace shiroDotnetRestfulDocker.Repositories
{
    public class UsersRepository
    {
        private readonly IMongoCollection<UserDetails> _usersCollection;

        public UsersRepository(IMongoClient mongoClient, IOptions<OrderJnjDatabaseSettings> settings)
        {
            var camelCaseConvention = new ConventionPack { new CamelCaseElementNameConvention() };
            ConventionRegistry.Register("CamelCase", camelCaseConvention, type => true);

            _usersCollection = mongoClient
                .GetDatabase(settings.Value.DatabaseName)
                .GetCollection<UserDetails>(settings.Value.UsersCollectionName);
        }

        public async Task<UserResponse> AddUserAsync(string username, string password,
           CancellationToken cancellationToken = default)
        {
            try
            {
                var newUser = new UserDetails();
                newUser.Username = username;
                newUser.Password = password;
                await _usersCollection.InsertOneAsync(newUser);
                var resultUser = await _usersCollection
                    .Find(Builders<UserDetails>.Filter.Eq(r => r.Username, username))
                    .FirstOrDefaultAsync();
                Console.WriteLine("Added new restaurant --- " + resultUser.ToJson());
                return new UserResponse();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                return new UserResponse();
            }
        }



    }
}
