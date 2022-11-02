using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger _logger;

        public UserCredentialsRepository(IMongoClient mongoClient, IOptions<OrderJnjDatabaseSettings> settings, ILogger<UserCredentialsRepository> logger)
        {
            var camelCaseConvention = new ConventionPack { new CamelCaseElementNameConvention() };
            ConventionRegistry.Register("CamelCase", camelCaseConvention, type => true);

            _userCredentialsRepository = mongoClient
                .GetDatabase(settings.Value.DatabaseName)
                .GetCollection<UserCredentials>(settings.Value.UserCredentialsCollectionName);

            _logger = logger;
        }

        public async Task<UserCredentialsResponse> AddUserCredentialsAsync(string username, string password,
           CancellationToken cancellationToken = default)
        {
            try
            {https://www.mongodb.com/docs/manual/reference/sql-comparison/
                _logger.LogInformation("AddUserCredentialsAsync called --- username = " + username + ", password = " + password);
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
                _logger.LogInformation("Added new UserCredentials --- " + resultUser.ToJson());
                return new UserCredentialsResponse(resultUser);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                return new UserCredentialsResponse(false, Constants.ERROR_LOGIN_FAILED);
            }
        }

        public async Task<UserCredentials> GetUserCredentialsAsync(string username,
         CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("GetUserCredentials called --- " + username);
            var resultUser = await _userCredentialsRepository
                .Find(Builders<UserCredentials>.Filter.Eq(r => r.UserName, username))
                .FirstOrDefaultAsync(cancellationToken);

            _logger.LogInformation("GetUserCredentials result --- " + resultUser);
            return resultUser;
        }

        public async Task<UserCredentialsResponse> LoginUserCredentialsAsync(UserCredentials userCredentials, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("LoginUserCredentialsAsync called --- " + userCredentials.ToJson());
                var newUser = new UserCredentials();
                var resultUser = await _userCredentialsRepository
                    .Find(Builders<UserCredentials>.Filter.Eq(r => r.UserName, userCredentials.UserName))
                    .FirstOrDefaultAsync(cancellationToken);

                if (resultUser == null)
                {
                    _logger.LogInformation("LoginUserCredentialsAsync Login failed -- user not exist");
                    return new UserCredentialsResponse(false, Constants.ERROR_LOGIN_FAILED);
                }

                _logger.LogInformation("LoginUserCredentialsAsync retrived user --- " + resultUser.ToJson());
                if (resultUser.Password.Equals(userCredentials.Password))
                {
                    _logger.LogInformation("LoginUserCredentialsAsync Login success");
                    return new UserCredentialsResponse(true, Constants.LOGIN_SUCCESS);
                }

                Console.WriteLine("LoginUserCredentialsAsync Login failed -- wrong passowrd");
                return new UserCredentialsResponse(false, Constants.ERROR_LOGIN_FAILED);
            }
            catch (Exception exception)
            {
                _logger.LogInformation(exception.ToJson());
                _logger.LogInformation("LoginUserCredentialsAsync Login failed -- user not found");
                return new UserCredentialsResponse(false, Constants.ERROR_LOGIN_FAILED);
            }
        }



    }
}
