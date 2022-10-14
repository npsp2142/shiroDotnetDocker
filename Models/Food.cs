using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace shiroDotnetRestfulDocker.Models
{
    public class Food
    {
        [BsonElement("_id")]
        [JsonPropertyName("_id")]
        [BsonId]
        public ObjectId Id { get; set; }
        public string NameChinese { get; set; } = string.Empty;
        public string NameEnglish { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<string> Posters { get; set; } = new List<string>();
        public float Price { get; set; }
        public string Remarks { get; set; } = string.Empty;


    }
}
