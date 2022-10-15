using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace shiroDotnetRestfulDocker.Models
{
    public class UserCredentials
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public DateTime CreationTime { get; set; }
        public DateTime LastAccessTime { get; set; }
        public DateTime LastModifiedTime { get; set; }
    }
}
