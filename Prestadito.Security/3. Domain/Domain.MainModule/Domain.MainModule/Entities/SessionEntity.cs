using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Prestadito.Security.Domain.MainModule.Entities
{
    public class SessionEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        [BsonElement("strUserId")]
        public string StrUserId { get; set; } = null!;
        [BsonElement("strIP")]
        public string StrIP { get; set; } = null!;
        [BsonElement("objDevice")]
        public ParameterEntity ObjDevice { get; set; } = null!;
        [BsonElement("objStatusLogin")]
        public ParameterEntity ObjStatusLogin { get; set; } = null!;
        [BsonElement("intAttempts")]
        public int IntAttempts { get; set; }
        [BsonElement("dteLogin")]
        public DateTime DteLogin { get; set; }
    }
}
