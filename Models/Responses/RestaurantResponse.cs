namespace shiroDotnetRestfulDocker.Models.Responses
{
    public class RestaurantResponse
    {
        private RestaurantResponse()
        {

        }
        public static RestaurantResponse Of(IReadOnlyList<Restaurant> restaurants, long count, int page)
        {
            RestaurantResponse result = new RestaurantResponse();
            result.Restaurants = restaurants;
            result.Count = count;
            result.Page = page;
            return result;
        }

        public static RestaurantResponse Of()
        {
            RestaurantResponse result = new RestaurantResponse();
            return result;
        }

        public IReadOnlyList<Restaurant> Restaurants { get; set; } = new List<Restaurant>();
        public long Count { get; set; }
        public int Page { get; set; }
    }


}
