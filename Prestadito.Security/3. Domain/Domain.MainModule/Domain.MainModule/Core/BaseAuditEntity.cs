﻿namespace Prestadito.Security.Domain.MainModule.Core
{
    public class BaseAuditEntity : AuditEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("blnActive")]
        public bool BlnActive { get; set; }
    }
}