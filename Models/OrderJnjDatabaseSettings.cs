namespace shiroDotnetRestfulDocker.Models
{
    public class OrderJnjDatabaseSettings
    {
        public const string OrderJnjDatabase = "OrderJnjDatabase";
        public string DatabaseName { get; set; } = null!;
        public string UserCredentialsCollectionName { get; set; } = null!;
        public string RestaurantsCollectionName { get; set; } = null!;
        public string FoodOrdersCollectionName { get; set; } = null!;
        public string UserProfilesCollectionName { get; set; } = null!;
    }
}
