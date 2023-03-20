namespace Prestadito.Security.Application.Dto.Login
{
    public class LoginRequest
    {
        public string StrEmail { get; set; } = string.Empty;
        public string StrPassword { get; set; } = string.Empty;
        public string StrDeviceName { get; set; } = string.Empty;
    }
}
