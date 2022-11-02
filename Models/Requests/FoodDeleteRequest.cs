namespace shiroDotnetRestfulDocker.Models.Requests
{
    public class FoodDeleteRequest
    {
        public string RestaurantId { get; set; } = string.Empty;

        public List<string> FoodNames { get; set; } = new List<string>();
    }
}
