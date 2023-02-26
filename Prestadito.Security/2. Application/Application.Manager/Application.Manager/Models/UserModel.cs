using Prestadito.Security.Domain.MainModule.Entities;

namespace Prestadito.Security.Application.Manager.Models
{
    public class UserModel
    {
        public string Id { get; set; } = string.Empty;
        public string StrDOI { get; set; } = string.Empty;
        public ParameterEntity ObjRol { get; set; } = null!;
        public bool BlnRegisterComplete { get; set; }
        public ParameterEntity ObjStatus { get; set; } = null!;
        public string StrEmail { get; set; } = string.Empty;
        public bool BlnActive { get; set; }
    }
}
