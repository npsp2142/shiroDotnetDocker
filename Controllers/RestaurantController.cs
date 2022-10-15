﻿using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using shiroDotnetRestfulDocker.Models;
using shiroDotnetRestfulDocker.Models.Requests;
using shiroDotnetRestfulDocker.Models.Responses;
using shiroDotnetRestfulDocker.Repositories;

namespace shiroDotnetRestfulDocker.Controllers
{
    public class RestaurantController
    {
        private readonly RestaurantsRepository _restaurantsRepository;
        private readonly FoodOrdersRepository _foodOrdersRepository;
        public RestaurantController(RestaurantsRepository _restaurantsRepository, FoodOrdersRepository foodOrdersRepository)
        {
            this._restaurantsRepository = _restaurantsRepository;
            _foodOrdersRepository = foodOrdersRepository;
        }


        [HttpPost("/api/v1/restaurants/register")]
        public async Task<ActionResult> AddRestaurant([FromBody] RestaurantAddRequest addRequest)
        {
            Dictionary<string, string> errors = new Dictionary<string, string>();

            Console.WriteLine("**** Start verification ****");
            if (addRequest.NameEn.Length < 3)
            {
                errors.Add("name", "Name too short.");
            }
            if (addRequest.Description.Length > 200)
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
                var restaurant = new Restaurant();
                restaurant.Id = ObjectId.GenerateNewId();
                restaurant.NameTc = addRequest.NameTc;
                restaurant.NameEn = addRequest.NameEn;
                restaurant.Description = addRequest.Description;
                restaurant.Address = addRequest.Address;
                restaurant.District = addRequest.District;
                restaurant.Region = addRequest.Region;
                restaurant.PriceLevel = addRequest.PriceLevel;
                restaurant.TelephoneNumber = addRequest.TelephoneNumber;
                restaurant.Posters = addRequest.Posters;
                restaurant.Remarks = addRequest.Remarks;
                restaurant.AvailableSeats = addRequest.AvailableSeats;
                await _restaurantsRepository.AddRestaurantAsync(restaurant);
                return new OkObjectResult(restaurant);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                return new BadRequestObjectResult(addRequest);
            }
        }


        [HttpGet("api/v1/restaurants/getorders")]
        public async Task<JsonResult> GetOrdersAsync(
            string restaurantId,
            int limit = 20,
            [FromQuery(Name = "page")] int page = 0,
            string sortKey = "creationTime",
            int sortOrder = -1,
            CancellationToken cancellationToken = default
            )
        {
            var orders = await _foodOrdersRepository.GetFoodOrdersByRestaurant(
                ObjectId.Parse(restaurantId), cancellationToken, sortKey, sortOrder, limit, page);

            //var orderCount = await _foodOrdersRepository.GetOrderCountAsync();
            var orderCount = 0;
            var foodOrderResponse = new FoodOrderResponse();
            foodOrderResponse.orderCount = orderCount;
            foodOrderResponse.orders = orders;
            foodOrderResponse.page = page;
            var okResult = new OkObjectResult(foodOrderResponse);

            return new JsonResult(okResult);
        }


        [HttpGet("api/v1/restaurants/")]
        public async Task<JsonResult> GetRestaurantsAsync(
         int limit = 20,
         [FromQuery(Name = "page")] int page = 0,
         string sortKey = "creationTime",
         int sortOrder = -1,
         CancellationToken cancellationToken = default
         )
        {
            var restaurants = await _restaurantsRepository.GetRestaurantsAsync(
                cancellationToken, sortKey, sortOrder, limit, page);

            var count = await _restaurantsRepository.GetRestaurantsCountAsync();
            var okResult = new OkObjectResult(RestaurantResponse.Of(restaurants, count, page));

            return new JsonResult(okResult);
        }
    }
}
