using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using shiroDotnetRestfulDocker.Models;
using shiroDotnetRestfulDocker.Models.Responses;

namespace shiroDotnetRestfulDocker.Repositories
{
    public class UserCredentialsRepository
    {
        private readonly IMongoCollection<UserCredentials> _usersCollection;

        public UserCredentialsRepository(IMongoClient mongoClient, IOptions<OrderJnjDatabaseSettings> settings)
        {
            var camelCaseConvention = new ConventionPack { new CamelCaseElementNameConvention() };
            ConventionRegistry.Register("CamelCase", camelCaseConvention, type => true);

            _usersCollection = mongoClient
                .GetDatabase(settings.Value.DatabaseName)
                .GetCollection<UserCredentials>(settings.Value.UsersCollectionName);
        }

        public async Task<UserResponse> AddUserAsync(string username, string password,
           CancellationToken cancellationToken = default)
        {
            try
            {
                var newUser = new UserCredentials();
                newUser.UserName = username;
                newUser.Password = password;
                await _usersCollection.InsertOneAsync(newUser);
                var resultUser = await _usersCollection
                    .Find(Builders<UserCredentials>.Filter.Eq(r => r.UserName, username))
                    .FirstOrDefaultAsync();
                Console.WriteLine("Added new UserCredentials --- " + resultUser.ToJson());
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
