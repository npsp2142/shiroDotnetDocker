using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace shiroDotnetRestfulDocker.Models
{
    public class Restaurant
    {
        [BsonElement("_id")]
        [JsonPropertyName("_id")]
        [BsonId]
        public ObjectId Id { get; set; }
        public string NameChinese { get; set; } = string.Empty;
        public string NameEnglish { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Address { get; set; }
        public string District { get; set; }
        public string Region { get; set; }
        public string PriceLevel { get; set; }
        public string TelephoneNumber { get; set; } = string.Empty;
        public DateTime LastModified { get; set; }
        public List<string> Posters { get; set; } = new List<string>();
        public string Remarks { get; set; } = string.Empty;
        public int AvailableSeats { get; set; }
        public List<Food> foods { get; set; } = new List<Food>();


    }
}
