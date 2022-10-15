using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using shiroDotnetRestfulDocker.Models;
using shiroDotnetRestfulDocker.Models.Responses;

namespace shiroDotnetRestfulDocker.Repositories
{
    public class UserProfilesRepository
    {
        private readonly IMongoCollection<UserProfile> _userProfilesCollection;

        public UserProfilesRepository(IMongoClient mongoClient, IOptions<OrderJnjDatabaseSettings> settings)
        {
            var camelCaseConvention = new ConventionPack { new CamelCaseElementNameConvention() };
            ConventionRegistry.Register("CamelCase", camelCaseConvention, type => true);

            _userProfilesCollection = mongoClient
                .GetDatabase(settings.Value.DatabaseName)
                .GetCollection<UserProfile>(settings.Value.UserProfilesCollectionName);
        }


        public async Task<UserProfileResponse> AddUserProfileAsync(UserProfile userProfile,
         CancellationToken cancellationToken = default)
        {
            try
            {
                await _userProfilesCollection.InsertOneAsync(userProfile);
                var resultUserProfile = await _userProfilesCollection
                    .Find(Builders<UserProfile>.Filter.Eq(r => r.UserId, userProfile.UserId))
                    .FirstOrDefaultAsync(cancellationToken);
                Console.WriteLine("Added new UserProfile --- " + resultUserProfile.ToJson());
                return UserProfileResponse.Of(resultUserProfile);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                return UserProfileResponse.Of(exception);
            }
        }
    }
}
