namespace shiroDotnetRestfulDocker.Models.Responses
{
    public class FoodOrderResponse
    {
        public IReadOnlyList<FoodOrder> orders { get; set; } = new List<FoodOrder>();
        public int orderCount { get; set; }
        public int page { get; set; }
    }
}
