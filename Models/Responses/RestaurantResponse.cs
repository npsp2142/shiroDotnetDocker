using System.Text.Json.Serialization;

namespace shiroDotnetRestfulDocker.Models.Responses
{
    public class RestaurantResponse
    {
        public RestaurantResponse()
        {
        }

        public RestaurantResponse(Restaurant restaurant)
        {
            Success = true;
            Restaurant = restaurant;
        }

        public RestaurantResponse(bool success, string message)
        {
            Success = success;
            if (success) SuccessMessage = message;
            else ErrorMessage = message;
        }

        public bool Success { get; set; }
        public string SuccessMessage { get; set; } = null!;
        public string ErrorMessage { get; set; } = null!;


        [JsonPropertyName("info")]
        public Restaurant Restaurant { get; set; }
    }


}
