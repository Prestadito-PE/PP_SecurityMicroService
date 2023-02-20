namespace Prestadito.Security.Application.Dto.User
{
    public class CreateUserDTO
    {
        public string StrEmail { get; set; } = string.Empty;
        public string StrPassword { get; set; } = string.Empty;
        public string StrRolCode { get; set; } = string.Empty;
    }
}
