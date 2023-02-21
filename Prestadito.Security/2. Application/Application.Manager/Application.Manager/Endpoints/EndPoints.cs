using Microsoft.AspNetCore.Builder;

namespace Prestadito.Security.Application.Manager.Endpoints
{
    public static class EndPoints
    {
        readonly static string basePath = "/api";
        public static WebApplication UseSecurityEndpoints(this WebApplication app, string cors)
        {
            app.UseHealthEndpoints();
            app.UseUserEndpoints(cors, basePath);
            return app;
        }
    }
}
