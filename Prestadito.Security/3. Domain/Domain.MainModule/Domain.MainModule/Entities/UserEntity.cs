using MongoDB.Bson.Serialization.Attributes;
using Prestadito.Security.Domain.MainModule.Core;

namespace Prestadito.Security.Domain.MainModule.Entities
{
    public class UserEntity : BaseAuditEntity
    {
        [BsonElement("strDOI")]
        public string StrDOI { get; set; } = string.Empty;
        [BsonElement("strPasswordHash")]
        public string StrPasswordHash { get; set; } = string.Empty;
        [BsonElement("objRol")]
        public ParameterEntity ObjRol { get; set; } = null!;
        [BsonElement("blnRegisterComplete")]
        public bool BlnRegisterComplete { get; set; }
        [BsonElement("objStatus")]
        public ParameterEntity ObjStatus { get; set; } = null!;
        [BsonElement("strEmail")]
        public string StrEmail { get; set; } = string.Empty;
    }
}
