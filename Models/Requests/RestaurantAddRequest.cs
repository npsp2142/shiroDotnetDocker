namespace shiroDotnetRestfulDocker.Models.Requests
{
    public class RestaurantAddRequest
    {
        public string NameTc { get; set; } = string.Empty;
        public string NameEn { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string District { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty;
        public string PriceLevel { get; set; } = string.Empty;
        public string TelephoneNumber { get; set; } = string.Empty;
        public List<string> Posters { get; set; } = new List<string>();
        public string Remarks { get; set; } = string.Empty;
        public int AvailableSeats { get; set; }
    }
}
