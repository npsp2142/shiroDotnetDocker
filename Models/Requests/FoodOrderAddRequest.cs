namespace shiroDotnetRestfulDocker.Models.Requests
{
    public class FoodOrderAddRequest
    {
        public string RestaurantId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string NameTc { get; set; } = string.Empty;
        public string NameEn { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<Food> ShoppingBasket { get; set; } = new List<Food>();

        public string Remarks { get; set; } = string.Empty;
    }
}
