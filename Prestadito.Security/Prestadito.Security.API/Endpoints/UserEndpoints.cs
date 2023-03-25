using FluentValidation;
using Prestadito.Security.Application.Dto.User.CreateUser;
using Prestadito.Security.Application.Dto.User.DeleteUser;
using Prestadito.Security.Application.Dto.User.DisableUser;
using Prestadito.Security.Application.Dto.User.GetUserById;
using Prestadito.Security.Application.Dto.User.UpdateUser;
using Prestadito.Security.Application.Manager.Interfaces;
using Prestadito.Security.Infrastructure.Data.Constants;

namespace Prestadito.Security.API.Endpoints
{
    public static class UserEndpoints
    {
        readonly static string collection = "users";
        public static WebApplication UseUserEndpoints(this WebApplication app, string basePath)
        {
            string path = $"{basePath}/{collection}";

            app.MapPost(path,
                async (IValidator<CreateUserRequest> validator, CreateUserRequest dto, IUsersController controller) =>
                {
                    var validationResult = await validator.ValidateAsync(dto);
                    if (!validationResult.IsValid)
                    {
                        return Results.ValidationProblem(validationResult.ToDictionary());
                    }
                    return await controller.CreateUser(dto, $"~{path}");
                }).WithTags(ConstantAPI.Endpoint.Tag.USERS);

            app.MapGet(path + "/all",
                async (IUsersController controller, HttpContext httpContext) =>
                {
                    return await controller.GetAllUsers();
                }).WithTags(ConstantAPI.Endpoint.Tag.USERS);

            app.MapGet(path,
                async (IUsersController controller) =>
                {
                    return await controller.GetActiveUsers();
                }).WithTags(ConstantAPI.Endpoint.Tag.USERS);

            app.MapGet(path + "/{id}",
                async (IValidator<GetUserByIdRequest> validator, string id, IUsersController controller) =>
                {
                    var request = new GetUserByIdRequest { StrId = id };
                    var validationResult = await validator.ValidateAsync(request);
                    if (!validationResult.IsValid)
                    {
                        return Results.ValidationProblem(validationResult.ToDictionary());
                    }
                    return await controller.GetUserById(request);
                }).WithTags(ConstantAPI.Endpoint.Tag.USERS);

            app.MapPut(path,
                async (IValidator<UpdateUserRequest> validator, UpdateUserRequest dto, IUsersController controller) =>
                {
                    var validationResult = await validator.ValidateAsync(dto);
                    if (!validationResult.IsValid)
                    {
                        return Results.ValidationProblem(validationResult.ToDictionary());
                    }
                    return await controller.UpdateUser(dto);
                }).WithTags(ConstantAPI.Endpoint.Tag.USERS);

            app.MapPut(path + "/disable/{id}",
                async (IValidator<DisableUserRequest> validator, string id, IUsersController controller) =>
                {
                    var request = new DisableUserRequest { StrId = id };
                    var validationResult = await validator.ValidateAsync(request);
                    if (!validationResult.IsValid)
                    {
                        return Results.ValidationProblem(validationResult.ToDictionary());
                    }
                    return await controller.DisableUser(id);
                }).WithTags(ConstantAPI.Endpoint.Tag.USERS);

            app.MapDelete(path + "/delete/{id}",
                async (IValidator<DeleteUserRequest> validator, string id, IUsersController controller) =>
                {
                    var request = new DeleteUserRequest { StrId = id };
                    var validationResult = await validator.ValidateAsync(request);
                    if (!validationResult.IsValid)
                    {
                        return Results.ValidationProblem(validationResult.ToDictionary());
                    }
                    return await controller.DeleteUser(request);
                }).WithTags(ConstantAPI.Endpoint.Tag.USERS);

            return app;
        }
    }
}
