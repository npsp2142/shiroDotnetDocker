namespace shiroDotnetRestfulDocker.Models
{
    public class OrderJnjDatabaseSettings
    {
        public const string OrderJnjDatabase = "OrderJnjDatabase";
        public string DatabaseName { get; set; } = null!;
        public string UsersCollectionName { get; set; } = null!;
        public string RestaurantsCollectionName { get; set; } = null!;
        public string FoodOrdersCollectionName { get; set; } = null!;
    }
}
