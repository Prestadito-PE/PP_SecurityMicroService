namespace Prestadito.Security.Application.Manager.Models
{
    public class UserModel
    {
        public string? Id { get; set; }
        public string StrUsername { get; set; } = null!;
        public Rol ObjRol { get; set; } = null!;
        public bool BlnActive { get; set; }
    }
}
