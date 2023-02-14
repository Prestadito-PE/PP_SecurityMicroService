namespace Prestadito.Security.Domain.MainModule.Entities
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        [BsonElement("strUsername")]
        public string StrUsername { get; set; } = null!;
        [BsonElement("strPassword")]
        public string StrPassword { get; set; } = null!;
        [BsonElement("objRol")]
        public Rol ObjRol { get; set; } = null!;
        [BsonElement("strCreateUser")]
        public string StrCreateUser { get; set; } = null!;
        [BsonElement("dteCreatedAt")]
        public DateTime DteCreatedAt { get; set; }
        [BsonElement("strUpdateUser")]
        public string StrUpdateUser { get; set; } = null!;
        [BsonElement("dteUpdatedAt")]
        public string DteUpdatedAt { get; set; } = null!;
        [BsonElement("blnActive")]
        public bool BlnActive { get; set; }
    }

    public class Rol
    {
        [BsonElement("strCode")]
        public string StrCode { get; set; } = null!;
        [BsonElement("strDescription")]
        public string StrDescription { get; set; } = null!;
    }
}
