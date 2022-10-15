using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace shiroDotnetRestfulDocker.Models
{
    public class UserDetails
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
        public string UserId { get; set; } = string.Empty;
        public string NameTc { get; set; } = string.Empty;
        public string NameEn { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string TelephoneNumber { get; set; } = string.Empty;
        public FoodOrder FoodOrderCurrent { get; set; } = new FoodOrder();
        public List<FoodOrder> FoodOrderHistory { get; set; } = new List<FoodOrder>();
        public DateTime CreationTime { get; set; }
        public DateTime LastAccessTime { get; set; }
        public DateTime LastModifiedTime { get; set; }
    }
}
