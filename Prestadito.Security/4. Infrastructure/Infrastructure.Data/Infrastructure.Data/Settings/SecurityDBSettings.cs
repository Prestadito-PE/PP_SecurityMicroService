using Prestadito.Security.Infrastructure.Data.Interface;

namespace Prestadito.Security.Infrastructure.Data.Settings
{
    public class SecurityDBSettings : ISecurityDBSettings
    {
        public string ConnectionURI { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
    }
}
