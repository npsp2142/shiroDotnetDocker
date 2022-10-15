using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace shiroDotnetRestfulDocker.Models
{
    public class Food
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string NameTc { get; set; } = string.Empty;
        public string NameEn { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<string> Posters { get; set; } = new List<string>();
        public float Price { get; set; }
        public string Remarks { get; set; } = string.Empty;


    }
}
