using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using shiroDotnetRestfulDocker.Models;
using System.Configuration;

using static shiroDotnetRestfulDocker.Models.RestaurantsSettings;
using ConfigurationManager = System.Configuration.ConfigurationManager;

namespace shiroDotnetRestfulDocker.Controllers
{
    public class RestaurantController
    {
        [HttpPost("/api/v1/restaurant/register")]
        public async Task<ActionResult> AddUser([FromBody] Restaurant restaurant)
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
            var _restaurantsCollection = database.GetCollection<Restaurant>("restaurants");

            Console.WriteLine("**** Start verification ****");
            if (restaurant.Name.Length < 3)
            {
                errors.Add("name", "Name too short.");
            }
            if (restaurant.Description.Length > 200)
            {
                errors.Add("description", "Description too long.");
            }
            if (errors.Count > 0)
            {
                var badRequest = new BadRequestResult();
                return badRequest;
            }
            try
            {
                var newRestaurant = new Restaurant();
                newRestaurant.Name = restaurant.Name;
                newRestaurant.Description = restaurant.Description;
                await _restaurantsCollection.InsertOneAsync(newRestaurant);
                var resultRestaurant = await _restaurantsCollection
                    .Find(Builders<Restaurant>.Filter.Eq(r => r.Name, restaurant.Name))
                    .FirstOrDefaultAsync();
                Console.WriteLine("Added new restaurant --- " + resultRestaurant.ToJson());
                return new OkResult();
            }catch (Exception exception)
            {
                Console.WriteLine(exception);
                return new BadRequestResult();
            }
        }
    }
}
