using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Text.Json.Serialization;

namespace shiroDotnetRestfulDocker.Models
{
    public class UserDetails
    {
        [BsonElement("_id")]
        [JsonPropertyName("_id")]
        [BsonId]
        public ObjectId Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
