using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace shiroDotnetRestfulDocker.Models
{
    public class UserProfile
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string NameTc { get; set; } = string.Empty;
        public string NameEn { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string TelephoneNumber { get; set; } = string.Empty;
        public FoodOrder? FoodOrderCurrent { get; set; }
        public List<FoodOrder> FoodOrderHistory { get; set; } = new List<FoodOrder>();
        public DateTime LastModifiedTime { get; set; }
    }
}
