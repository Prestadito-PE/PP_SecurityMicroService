using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Prestadito.Security.Application.Dto.Login;
using Prestadito.Security.Application.Manager.Interfaces;

namespace Prestadito.Security.Application.Manager.Endpoints
{
    public static class SessionEndpoints
    {
        readonly static string collection = "sessions";
        public static WebApplication UseSessionEndpoints(this WebApplication app, string basePath)
        {
            string path = $"{basePath}/{collection}";

            app.MapPost(path + "/login",
                async (IValidator<LoginDTO> validator, LoginDTO dto, ISessionsController controller, HttpContext httpContext) =>
                {
                    var validationResult = await validator.ValidateAsync(dto);
                    if (!validationResult.IsValid)
                    {
                        return Results.ValidationProblem(validationResult.ToDictionary());
                    }
                    return await controller.Login(dto, httpContext);
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
