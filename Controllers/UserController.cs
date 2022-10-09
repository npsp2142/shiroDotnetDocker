using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using shiroDotnetRestfulDocker.Models;
using System.Configuration;
using ConfigurationManager = System.Configuration.ConfigurationManager;

namespace shiroDotnetRestfulDocker.Controllers
{
    public class UserController : Controller
    {
        [HttpPost("/api/v1/user/register")]
        public async Task<ActionResult> AddUser([FromBody] UserDetails userDetails)
        {
            Dictionary<string, string> errors = new Dictionary<string, string>();


            // Get the current configuration file.
            Configuration config =
                    ConfigurationManager.OpenExeConfiguration(
                    ConfigurationUserLevel.None);

            var builder = WebApplication.CreateBuilder();
            var restaurantsApiKey = builder.Configuration["Restaurants:ServiceApiKey"];
            var restaurantsConnectionUrl = builder.Configuration["Restaurants:ConnectionUrl"];

            var mongoUrlBuilder = new MongoUrlBuilder();
            mongoUrlBuilder.Parse(restaurantsConnectionUrl);
            mongoUrlBuilder.Username = "admin";
            mongoUrlBuilder.Password = restaurantsApiKey;
            var settings = MongoClientSettings.FromConnectionString(mongoUrlBuilder.ToString());
            settings.ServerApi = new ServerApi(ServerApiVersion.V1);
            var client = new MongoClient(settings);
            var database = client.GetDatabase("orderjnj");
            var _usersCollection = database.GetCollection<UserDetails>("users");

            Console.WriteLine("**** Start verification ****");
            if (userDetails.Username.Length < 3)
            {
                errors.Add("Username", "Username too short.");
            }
            if (userDetails.Password.Length < 3)
            {
                errors.Add("Password", "Password too short.");
            }
            if (errors.Count > 0)
            {
                var badRequest = new BadRequestResult();
                return badRequest;
            }
            try
            {
                var newUser = new UserDetails();
                newUser.Username = userDetails.Username;
                newUser.Password = userDetails.Password;
                await _usersCollection.InsertOneAsync(newUser);
                var resultUser = await _usersCollection
                    .Find(Builders<UserDetails>.Filter.Eq(r => r.Username, userDetails.Username))
                    .FirstOrDefaultAsync();
                Console.WriteLine("Added new restaurant --- " + resultUser.ToJson());
                return new OkResult();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                return new BadRequestResult();
            }
        }
    }
}
