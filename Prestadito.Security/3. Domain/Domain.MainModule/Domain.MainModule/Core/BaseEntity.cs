using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Prestadito.Security.Domain.MainModule.Core
{
    public class BaseEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("blnActive")]
        public bool BlnActive { get; set; }
    }
}
