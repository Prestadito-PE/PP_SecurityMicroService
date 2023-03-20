using Microsoft.AspNetCore.Builder;

namespace Prestadito.Security.API.Endpoints
{
    public static class HealthEndpoint
    {
        readonly static string path = "/api/health";
        public static WebApplication UseHealthEndpoints(this WebApplication app)
        {
            app.UseHealthChecks(path);
            return app;
        }
    }
}
