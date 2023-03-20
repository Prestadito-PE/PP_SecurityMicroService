namespace Prestadito.Security.Infrastructure.Data.Interface
{
    public interface IJWTSettings
    {
        string SecretKey { get; set; }
        string Issuer { get; set; }
        string Audience { get; set; }
        int ExpirationInMinutes { get; set; }
    }
}
