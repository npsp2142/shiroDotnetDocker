using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace shiroDotnetRestfulDocker.Models
{
    public class Restaurant
    {
        private string _id = string.Empty;
        [BsonElement("_id")]
        [JsonPropertyName("_id")]
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id
        {
            get { return this._id; }
            set { this._id = value; }
        }
        public string NameTc { get; set; } = string.Empty;
        public string NameEn { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string District { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty;
        public string PriceLevel { get; set; } = string.Empty;
        public string TelephoneNumber { get; set; } = string.Empty;
        public DateTime LastModified { get; set; }
        public List<string> Posters { get; set; } = new List<string>();
        public string Remarks { get; set; } = string.Empty;
        public int AvailableSeats { get; set; }
        public List<Food> Menu { get; set; } = new List<Food>();
        public List<FoodOrder> FoodOrdersCurrent { get; set; } = new List<FoodOrder>();
        public List<FoodOrder> FoodOrdersHistory { get; set; } = new List<FoodOrder>();
    }
}
