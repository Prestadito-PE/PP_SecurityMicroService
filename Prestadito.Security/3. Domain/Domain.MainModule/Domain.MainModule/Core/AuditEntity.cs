using MongoDB.Bson.Serialization.Attributes;

namespace Prestadito.Security.Domain.MainModule.Core
{
    public class AuditEntity : BaseEntity
    {
        [BsonElement("strCreateUser")]
        public string StrCreateUser { get; set; } = null!;
        [BsonElement("dteCreatedAt")]
        public DateTime DteCreatedAt { get; set; } = DateTime.UtcNow;
        [BsonElement("strUpdateUser")]
        public string StrUpdateUser { get; set; } = null!;
        [BsonElement("dteUpdatedAt")]
        public DateTime DteUpdatedAt { get; set; }
    }
}
