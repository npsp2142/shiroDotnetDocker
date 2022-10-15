namespace shiroDotnetRestfulDocker.Models.Responses
{
    public class FoodOrderResponse
    {
        public IReadOnlyList<FoodOrder> orders { get; set; }
        public int orderCount { get; set; }
        public int page { get; set; }

        public FoodOrderResponse()
        {

        }

        public FoodOrderResponse(IReadOnlyList<FoodOrder> orders, int orderCount, int page)
        {
            this.orders = orders;
            this.orderCount = orderCount;
            this.page = page;
        }
    }
}
