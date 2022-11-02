namespace shiroDotnetRestfulDocker.Models.Requests
{
    public class FoodAddRequest
    {
        /*public string NameTc { get; set; } = string.Empty;
        public string NameEn { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public float Price { get; set; }
        public string Remarks { get; set; } = string.Empty;*/

        public string RestaurantId { get; set; } = string.Empty;

        public List<Food> Foods { get; set; } = new List<Food>();

    }
}
