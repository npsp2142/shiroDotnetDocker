namespace shiroDotnetRestfulDocker.Models.Responses
{
    public class FoodOrderResponse
    {
        public FoodOrderResponse(FoodOrder order)
        {
            Success = true;
            var orders = new List<FoodOrder>();
            orders.Add(order);
            Orders = orders;
        }
        public FoodOrderResponse(IReadOnlyList<FoodOrder> orders, int count, int page)
        {
            Success = true;
            Orders = orders;
        }
        public FoodOrderResponse(bool success, string message)
        {
            Success = success;
            Message = message;
        }
        public IReadOnlyList<FoodOrder> Orders { get; set; } = new List<FoodOrder>();
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public int Count { get; set; }
        public int Page { get; set; }
    }
}
