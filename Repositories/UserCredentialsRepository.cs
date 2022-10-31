using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using shiroDotnetRestfulDocker.Models;
using shiroDotnetRestfulDocker.Models.Responses;
using System.Threading;

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

        public async Task<UserCredentialsResponse> AddUserCredentialsAsync(string username, string password,
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
                Console.WriteLine("Added new UserCredentialsResponse --- " + resultUser);
                return new UserCredentialsResponse(true, resultUser.UserName);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                return new UserCredentialsResponse(false,Constants.ERROR_LOGIN_FAILED);
            }
        }

        public async Task<UserCredentials> GetUserCredentialsAsync(string username,
         CancellationToken cancellationToken = default)
        {
            Console.WriteLine("GetUserCredentials called --- " + username);
            var resultUser = await _userCredentialsRepository
                .Find(Builders<UserCredentials>.Filter.Eq(r => r.UserName, username))
                .FirstOrDefaultAsync(cancellationToken);

            Console.WriteLine("GetUserCredentials result --- " + resultUser);
            return resultUser;
        }

        public async Task<UserCredentialsResponse> LoginUserCredentialsAsync(UserCredentials userCredentials, CancellationToken cancellationToken = default)
        {
            try
            {
                Console.WriteLine("LoginUserCredentialsAsync called --- " + userCredentials.ToJson());
                var newUser = new UserCredentials();
                var resultUser = await _userCredentialsRepository
                    .Find(Builders<UserCredentials>.Filter.Eq(r => r.UserName, userCredentials.UserName))
                    .FirstOrDefaultAsync(cancellationToken);

                Console.WriteLine("LoginUserCredentialsAsync retrived user --- " + resultUser.ToJson());
                if (resultUser.Password.Equals(userCredentials.Password))
                {
                    Console.WriteLine("LoginUserCredentialsAsync Login success");
                    return new UserCredentialsResponse(true, Constants.LOGIN_SUCCESS);
                }

                Console.WriteLine("LoginUserCredentialsAsync Login failed -- wrong passowrd");
                return new UserCredentialsResponse(false, Constants.ERROR_LOGIN_FAILED);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                Console.WriteLine("LoginUserCredentialsAsync Login failed -- user not found");
                return new UserCredentialsResponse(false, Constants.ERROR_LOGIN_FAILED);
            }
        }



    }
}
