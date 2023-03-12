using MongoDB.Bson.Serialization.Attributes;
using Prestadito.Security.Domain.MainModule.Core;

namespace Prestadito.Security.Domain.MainModule.Entities
{
    [BsonIgnoreExtraElements]
    public class UserEntity : AuditEntity
    {
        [BsonElement("strDOI")]
        public string StrDOI { get; set; } = string.Empty;
        [BsonElement("strPasswordHash")]
        public string StrPasswordHash { get; set; } = string.Empty;
        [BsonElement("strRolId")]
        public string StrRolId { get; set; } = string.Empty;
        [BsonElement("blnEmailValitated")]
        public bool BlnEmailValitated { get; set; }
        [BsonElement("strStatusId")]
        public string StrStatusId { get; set; } = string.Empty;
        [BsonElement("strEmail")]
        public string StrEmail { get; set; } = string.Empty;
    }
}
