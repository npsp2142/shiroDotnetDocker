using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using shiroDotnetRestfulDocker.Models;
using shiroDotnetRestfulDocker.Repositories;
using System.Configuration;

using ConfigurationManager = System.Configuration.ConfigurationManager;

namespace shiroDotnetRestfulDocker.Controllers
{
    public class RestaurantController
    {
        private readonly RestaurantsRepository _restaurantsRepository;

        public RestaurantController(RestaurantsRepository _restaurantsRepository)
        {
            this._restaurantsRepository = _restaurantsRepository;
        }


        [HttpPost("/api/v1/restaurant/register")]
        public async Task<ActionResult> AddUser([FromBody] Restaurant restaurant)
        {
            Dictionary<string, string> errors = new Dictionary<string, string>();

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
                await _restaurantsRepository.AddRestaurantAsync(restaurant.Name, restaurant.Description);
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
