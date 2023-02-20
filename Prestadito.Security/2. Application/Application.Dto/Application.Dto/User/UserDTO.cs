using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prestadito.Security.Application.Dto.User
{
    public class UserDTO
    {
        public string? Id { get; set; }
        public string StrDOI { get; set; } = null!;
        public ParameterEntity ObjRol { get; set; } = null!;
        public bool BlnRegisterComplete { get; set; }
        public ParameterEntity ObjStatus { get; set; } = null!;
        public string StrEmail { get; set; } = null!;
        public bool BlnActive { get; set; }
    }
}
