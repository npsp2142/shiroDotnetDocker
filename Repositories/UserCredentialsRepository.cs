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
        private readonly IMongoCollection<UserCredentials> _userCredentialsRepository;

        public UserCredentialsRepository(IMongoClient mongoClient, IOptions<OrderJnjDatabaseSettings> settings)
        {
            var camelCaseConvention = new ConventionPack { new CamelCaseElementNameConvention() };
            ConventionRegistry.Register("CamelCase", camelCaseConvention, type => true);

            _userCredentialsRepository = mongoClient
                .GetDatabase(settings.Value.DatabaseName)
                .GetCollection<UserCredentials>(settings.Value.UserCredentialsCollectionName);
        }

        public async Task<UserCredentialsResponse> AddUserAsync(string username, string password,
           CancellationToken cancellationToken = default)
        {
            try
            {
                var newUser = new UserCredentials();
                newUser.Id = ObjectId.GenerateNewId();
                newUser.UserName = username;
                newUser.Password = password;
                newUser.CreationTime = DateTime.UtcNow;
                newUser.LastModifiedTime = DateTime.UtcNow;
                await _userCredentialsRepository.InsertOneAsync(newUser);

                var resultUser = await _userCredentialsRepository
                    .Find(Builders<UserCredentials>.Filter.Eq(r => r.UserName, username))
                    .FirstOrDefaultAsync(cancellationToken);
                Console.WriteLine("Added new UserCredentials --- " + resultUser.ToJson());
                Console.WriteLine("Added new UserCredentialsResponse --- " + UserCredentialsResponse.Of(resultUser.Id.ToString(), resultUser.UserName).ToJson());
                return UserCredentialsResponse.Of(resultUser.Id.ToString(), resultUser.UserName);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                return UserCredentialsResponse.Of();
            }
        }



    }
}
