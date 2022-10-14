using Microsoft.AspNetCore.Mvc;
using shiroDotnetRestfulDocker.Models;
using shiroDotnetRestfulDocker.Repositories;

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
        public async Task<ActionResult> AddRestaurant([FromBody] Restaurant restaurant)
        {
            Dictionary<string, string> errors = new Dictionary<string, string>();

            Console.WriteLine("**** Start verification ****");
            if (restaurant.NameEnglish.Length < 3)
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

            Console.WriteLine("**** Start insert ****");
            try
            {
                await _restaurantsRepository.AddRestaurantAsync(restaurant);
                return new OkObjectResult(restaurant);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                return new BadRequestObjectResult(restaurant);
            }
        }
    }
}
