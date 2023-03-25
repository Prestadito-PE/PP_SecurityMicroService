namespace Prestadito.Security.Application.Dto.User.DeleteUser
{
    public class DeleteUserResponse
    {
        public string StrId { get; set; } = string.Empty;
        public string StrDOI { get; set; } = string.Empty;
        public string StrRolId { get; set; } = string.Empty;
        public bool BlnEmailValidated { get; set; }
        public string StrStatusId { get; set; } = string.Empty;
        public string StrEmail { get; set; } = string.Empty;
    }
}
