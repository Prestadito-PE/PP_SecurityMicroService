namespace Prestadito.Security.Application.Manager.Models
{
    public class UserModel
    {
        public string Id { get; set; } = string.Empty;
        public string StrDOI { get; set; } = string.Empty;
        public string StrRolId { get; set; } = null!;
        public bool BlnEmailValitated { get; set; }
        public string StrStatusId { get; set; } = null!;
        public string StrEmail { get; set; } = string.Empty;
        public bool BlnActive { get; set; }
    }
}
