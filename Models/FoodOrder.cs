using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace shiroDotnetRestfulDocker.Models
{
    public class FoodOrder
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public ObjectId RestaurantId { get; set; }
        public ObjectId UserId { get; set; }
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
