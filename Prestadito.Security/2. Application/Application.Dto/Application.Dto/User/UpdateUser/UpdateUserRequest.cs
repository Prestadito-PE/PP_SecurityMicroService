namespace Prestadito.Security.Application.Dto.User.UpdateUser
{
    public class UpdateUserRequest
    {
        public string Id { get; set; } = string.Empty;
        public string StrDOI { get; set; } = string.Empty;
        public string StrPassword { get; set; } = string.Empty;
        public string StrRolId { get; set; } = string.Empty;
    }
}
