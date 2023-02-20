namespace Prestadito.Security.Application.Manager.Endpoints
{
    public static class EndPoints
    {
        readonly static string basePath = "/api";
        public static WebApplication UseEndpoints(this WebApplication app, string cors)
        {
            app.UseHealthEndpoints();
            app.UseUserEndpoints(cors, basePath);
            return app;
        }
    }
}
