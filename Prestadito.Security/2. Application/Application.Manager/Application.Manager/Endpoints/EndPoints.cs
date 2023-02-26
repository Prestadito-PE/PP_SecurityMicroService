using Microsoft.AspNetCore.Builder;

namespace Prestadito.Security.Application.Manager.Endpoints
{
    public static class EndPoints
    {
        readonly static string basePath = "/api";
        public static WebApplication UseSecurityEndpoints(this WebApplication app)
        {
            app.UseHealthEndpoints();
            app.UseUserEndpoints(basePath);
            return app;
        }
    }
}
