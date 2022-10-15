using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace shiroDotnetRestfulDocker.Models
{
    public class FoodOrder
    {
        private string _id;
        [BsonElement("_id")]
        [JsonPropertyName("_id")]
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id
        {
            get { return this._id; }
            set { this._id = value; }
        }
        public ObjectId RestaurantId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string NameTc { get; set; } = string.Empty;
        public string NameEn { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<Food> ShoppingBasket { get; set; } = new List<Food>();
        public string Remarks { get; set; } = OrderStatus.Nil.ToString();
        public string Status { get; set; } = string.Empty;
        public DateTime LastModified { get; set; }
        public DateTime CreationTime { get; set; }

        public enum OrderStatus
        {
            Nil, Pending, Cancelled, Completed
        }
    }
}
