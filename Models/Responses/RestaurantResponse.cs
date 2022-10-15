namespace shiroDotnetRestfulDocker.Models.Responses
{
    public class RestaurantResponse
    {
        public RestaurantResponse()
        {
        }

        public RestaurantResponse(IReadOnlyList<Restaurant> restaurants, long count, int page)
        {
            Restaurants = restaurants;
            Count = count;
            Page = page;
        }

        public IReadOnlyList<Restaurant> Restaurants { get; }
        public long Count { get; }
        public int Page { get; }
    }


}
