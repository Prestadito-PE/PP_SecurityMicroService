using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Prestadito.Security.Domain.MainModule.Core;

namespace Prestadito.Security.Domain.MainModule.Entities
{
    [BsonIgnoreExtraElements]
    public class SessionEntity : AuditEntity
    {
        [BsonElement("strUserId")]
        public string StrUserId { get; set; } = null!;
        [BsonElement("strIP")]
        public string StrIP { get; set; } = null!;
        [BsonElement("strDeviceName")]
        public string StrDeviceName { get; set; } = null!;
        [BsonElement("intAttempts")]
        public int IntAttempts { get; set; } = 0;
        [BsonElement("strComment")]
        public string StrComment { get; set; } = null!;
        [BsonElement("strEnteredPasswordHash")]
        public string StrEnteredPasswordHash { get; set; } = null!;
        [BsonElement("dteLogin")]
        public DateTime DteLogin { get; set; } = DateTime.UtcNow;
    }
}
