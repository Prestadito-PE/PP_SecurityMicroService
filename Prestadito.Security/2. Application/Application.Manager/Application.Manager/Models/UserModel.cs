namespace Prestadito.Security.Application.Manager.Models
{
    public class UserModel
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
