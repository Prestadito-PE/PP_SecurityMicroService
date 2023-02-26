using MongoDB.Bson.Serialization.Attributes;

namespace Prestadito.Security.Domain.MainModule.Entities
{
    public class ParameterEntity
    {
        [BsonElement("strCode")]
        public string StrCode { get; set; } = null!;
        [BsonElement("strDescription")]
        public string StrDescription { get; set; } = null!;
    }
}
