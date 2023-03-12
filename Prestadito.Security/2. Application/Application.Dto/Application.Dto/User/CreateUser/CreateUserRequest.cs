namespace Prestadito.Security.Application.Dto.User.CreateUser
{
    public class CreateUserRequest
    {
        public string StrEmail { get; set; } = string.Empty;
        public string StrPassword { get; set; } = string.Empty;
        public string StrRolId { get; set; } = string.Empty;
    }
}
