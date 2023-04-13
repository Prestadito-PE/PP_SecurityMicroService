using FluentValidation;
using Prestadito.Security.Application.Dto.Session.Login;
using Prestadito.Security.Application.Dto.User.GetUserById;
using Prestadito.Security.Application.Manager.Interfaces;
using Prestadito.Security.Infrastructure.Data.Constants;

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
                }).WithTags(ConstantAPI.Endpoint.Tag.SESSIONS);

            app.MapGet(path + "/all",
                async (ISessionsController controller) =>
                {
                    return await controller.GetAllSessions();
                }).WithTags(ConstantAPI.Endpoint.Tag.SESSIONS);

            app.MapDelete(path + "/delete/{id}",
                async (IValidator<DeleteSessionRequest> validator, string id, ISessionsController controller) =>
                {
                    var request = new DeleteSessionRequest { StrId = id };
                    var validationResult = await validator.ValidateAsync(request);
                    if (!validationResult.IsValid)
                    {
                        return Results.ValidationProblem(validationResult.ToDictionary());
                    }
                    return await controller.DeleteSession(request);
                }).WithTags(ConstantAPI.Endpoint.Tag.SESSIONS);

            return app;
        }
    }
}
