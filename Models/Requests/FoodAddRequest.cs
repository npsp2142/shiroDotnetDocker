namespace shiroDotnetRestfulDocker.Models.Requests
{
    public class FoodAddRequest
    {
        public string NameTc { get; set; } = string.Empty;
        public string NameEn { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public float Price { get; set; }
        public string Remarks { get; set; } = string.Empty;

    }
}
