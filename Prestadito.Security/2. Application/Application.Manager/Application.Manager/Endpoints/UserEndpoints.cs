using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Prestadito.Security.Application.Dto.Login;
using Prestadito.Security.Application.Dto.User;
using Prestadito.Security.Application.Manager.Interfaces;

namespace Prestadito.Security.Application.Manager.Endpoints
{
    public static class UserEndpoints
    {
        readonly static string collection = "users";
        public static WebApplication UseUserEndpoints(this WebApplication app, string basePath)
        {
            string path = string.Format("{0}/{1}", basePath, collection);

            app.MapPost(path + "/login",
                async (LoginDTO dto, IUsersController controller) =>
                {
                    return await controller.Login(dto);
                });

            app.MapPost(path,
                async (IValidator<CreateUserDTO> validator, CreateUserDTO dto, IUsersController controller) =>
                {
                    var validationResult = await validator.ValidateAsync(dto);
                    if (!validationResult.IsValid)
                    {
                        return Results.ValidationProblem(validationResult.ToDictionary());
                    }
                    return await controller.CreateUser(dto, string.Format("~/{0}", path));
                });

            app.MapGet(path + "/all",
                async (IUsersController controller) =>
                {
                    return await controller.GetAllUsers();
                });

            app.MapGet(path,
                async (IUsersController controller) =>
                {
                    return await controller.GetActiveUsers();
                });

            app.MapGet(path + "/{id}",
                async (string id, IUsersController controller) =>
                {
                    return await controller.GetUserById(id);
                });

            app.MapPut(path,
                async (IValidator<UpdateUserDTO> validator, UpdateUserDTO dto, IUsersController controller) =>
                {
                    var validationResult = await validator.ValidateAsync(dto);
                    if (!validationResult.IsValid)
                    {
                        return Results.ValidationProblem(validationResult.ToDictionary());
                    }
                    return await controller.UpdateUser(dto);
                });

            app.MapPut(path + "/disable/{id}",
                async (string id, IUsersController controller) =>
                {
                    return await controller.DisableUser(id);
                });

            app.MapDelete(path + "/delete/{id}",
                async (string id, IUsersController controller) =>
                {
                    return await controller.DeleteUser(id);
                });

            return app;
        }
    }
}
