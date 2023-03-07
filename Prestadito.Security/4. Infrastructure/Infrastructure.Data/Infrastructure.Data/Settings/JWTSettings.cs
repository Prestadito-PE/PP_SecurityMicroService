using Prestadito.Security.Infrastructure.Data.Interface;

namespace Prestadito.Security.Infrastructure.Data.Settings
{
    public class JWTSettings : IJWTSettings
    {
        public string SecretKey { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public int ExpirationInMinutes { get; set; }
    }
}
