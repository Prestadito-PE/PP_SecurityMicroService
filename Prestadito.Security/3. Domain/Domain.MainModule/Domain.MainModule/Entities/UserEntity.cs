using MongoDB.Bson.Serialization.Attributes;
using Prestadito.Security.Domain.MainModule.Core;

namespace Prestadito.Security.Domain.MainModule.Entities
{
    [BsonIgnoreExtraElements]
    public class UserEntity : AuditEntity
    {
        [BsonElement("strEmail")]
        public string StrEmail { get; set; } = string.Empty;
        [BsonElement("strPasswordHash")]
        public string StrPasswordHash { get; set; } = string.Empty;
        [BsonElement("strRolId")]
        public string StrRolId { get; set; } = string.Empty;
        [BsonElement("blnEmailValidated")]
        public bool BlnEmailValidated { get; set; }
        [BsonElement("blnLockByAttempts")]
        public bool BlnLockByAttempts { get; set; }
        [BsonElement("blnCompleteInformation")]
        public bool BlnCompleteInformation { get; set; }
    }
}
