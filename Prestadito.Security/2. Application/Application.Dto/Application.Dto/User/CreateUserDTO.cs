namespace Prestadito.Security.Application.Dto.User
{
    public class CreateUserDTO
    {
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string RolCode { get; set; } = null!;
    }
}
