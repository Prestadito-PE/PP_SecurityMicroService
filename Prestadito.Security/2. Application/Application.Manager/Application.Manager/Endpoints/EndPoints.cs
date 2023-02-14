namespace Prestadito.Security.Application.Manager.Endpoints
{
    public static class EndPoints
    {
        public static WebApplication UseEndpoints(this WebApplication app)
        {
            app.UseHealthEndpoints();
            app.UseSecurityEndpoints();
            return app;
        }
    }
}
