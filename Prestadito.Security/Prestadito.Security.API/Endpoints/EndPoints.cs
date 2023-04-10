namespace Prestadito.Security.API.Endpoints
{
    public static class EndPoints
    {
        readonly static string basePath = "/api";
        public static WebApplication UseSecurityEndpoints(this WebApplication app)
        {
            app.UseUserEndpoints(basePath);
            app.UseSessionEndpoints(basePath);

            return app;
        }
    }
}
