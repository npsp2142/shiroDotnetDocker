using System.Collections.Generic;

namespace shiroDotnetRestfulDocker.Models.Responses
{
    public class RestaurantResponse
    {
        public RestaurantResponse(IReadOnlyList<Restaurant> restaurants, long count, int page)
        {
            Success = true;
            Restaurants = restaurants;
        }

        public RestaurantResponse(Restaurant restaurant)
        {
            Success = true;
            var restaurants = new List<Restaurant>();
            restaurants.Add(restaurant);
            Restaurants = restaurants;
        }

        public RestaurantResponse(bool success, string message)
        {
            Success = success;
            Message = message;
        }

        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;

        public IReadOnlyList<Restaurant> Restaurants { get; set; } = new List<Restaurant>();
        public long Count { get; set; }
        public int Page { get; set; }
    }


}
