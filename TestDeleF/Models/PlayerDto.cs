using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TestDeleF.Models
{
    public class PlayerDto(string id, string name, long highscore, long expieryTime)
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = id;

        [BsonElement("Name")]
        public string Name { get; set; } = name;

        public long Highscore { get; set; } = highscore;

        public long ExpieryTime { get; set; } = expieryTime;
    }
}
