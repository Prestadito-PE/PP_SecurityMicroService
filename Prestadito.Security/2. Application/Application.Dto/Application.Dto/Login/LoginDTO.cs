namespace Prestadito.Security.Application.Dto.Login
{
    public class LoginDTO
    {
        public string StrEmail { get; set; } = null!;
        public string StrPasswordHash { get; set; } = null!;
    }
}
