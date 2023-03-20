using FluentValidation;
using Prestadito.Security.Application.Dto.Login;
using Prestadito.Security.Application.Manager.Interfaces;

namespace Prestadito.Security.API.Endpoints
{
    public static class SessionEndpoints
    {
        readonly static string collection = "sessions";
        public static WebApplication UseSessionEndpoints(this WebApplication app, string basePath)
        {
            string path = $"{basePath}/{collection}";

            app.MapPost(path + "/login",
                async (IValidator<LoginRequest> validator, LoginRequest request, ISessionsController controller, HttpContext httpContext) =>
                {
                    var validationResult = await validator.ValidateAsync(request);
                    if (!validationResult.IsValid)
                    {
                        return Results.ValidationProblem(validationResult.ToDictionary());
                    }
                    return await controller.Login(request, httpContext);
                });

            app.MapGet(path + "/all",
                async (ISessionsController controller, HttpContext httpContext) =>
                {
                    return await controller.GetAllSessions();
                });


            app.MapDelete(path + "/delete/{id}",
                async (string id, ISessionsController controller) =>
                {
                    return await controller.DeleteSession(id);
                });

            return app;
        }
    }
}
