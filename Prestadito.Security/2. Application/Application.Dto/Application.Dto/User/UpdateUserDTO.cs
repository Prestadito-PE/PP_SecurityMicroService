namespace Prestadito.Security.Application.Dto.User
{
    public class UpdateUserDTO
    {
        public string Id { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string RolCode { get; set; } = null!;
    }
}
