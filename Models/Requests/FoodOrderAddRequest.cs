namespace shiroDotnetRestfulDocker.Models.Requests
{
    public class FoodOrderAddRequest
    {
        public string RestaurantId { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string NameTc { get; set; } = string.Empty;
        public string NameEn { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<FoodAddRequest> ShoppingBasket { get; set; } = new List<FoodAddRequest>();

        public string Remarks { get; set; } = string.Empty;
    }
}
