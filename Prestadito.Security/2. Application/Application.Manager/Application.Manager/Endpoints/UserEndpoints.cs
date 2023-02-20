using Prestadito.Security.Application.Dto.Login;

namespace Prestadito.Security.Application.Manager.Endpoints
{
    public static class UserEndpoints
    {
        private static string myCors = "myCors";
        readonly static string path = "/api/security";
        public static WebApplication UseUserEndpoints(this WebApplication app)
        {
            string complementPath = "/users";

            app.MapPost(path + complementPath + "UserAuthentication",
                async (LoginDTO dto, IUsersController controller) =>
                {
                    var response = await controller.UserAuthentication(dto);
                    return response != null ? Results.Ok(response) : Results.UnprocessableEntity(response);
                }).RequireCors(myCors);

            app.MapPost(path + complementPath,
                async (IValidator<CreateUserDTO> validator, CreateUserDTO dto, IUsersController controller) =>
                {
                    var validationResult = await validator.ValidateAsync(dto);
                    if (!validationResult.IsValid)
                    {
                        return Results.ValidationProblem(validationResult.ToDictionary());
                    }
                    var response = await controller.CreateUser(dto);
                    return response != null ? Results.Ok(response) : Results.UnprocessableEntity(response);
                }).RequireCors(myCors);

            app.MapGet(path + complementPath,
                async (IUsersController controller) =>
                {
                    var response = await controller.GetActiveUsers();
                    return response != null && response.Items != null ? Results.Ok(response) : Results.UnprocessableEntity(response);
                }).RequireCors(myCors);

            app.MapGet(path + complementPath + "/{id}",
                async (string id, IUsersController controller) =>
                {
                    var response = await controller.GetUserById(id);
                    return response != null ? Results.Ok(response) : Results.UnprocessableEntity(response);
                }).RequireCors(myCors);

            app.MapPut(path + complementPath,
                async (UpdateUserDTO dto, IUsersController controller) =>
                {
                    var response = await controller.UpdateUser(dto);
                    return response != null ? Results.Ok(response) : Results.UnprocessableEntity(response);
                }).RequireCors(myCors);

            app.MapPut(path + complementPath + "/delete/{id}",
                async (string id, IUsersController controller) =>
                {
                    var response = await controller.DeleteLogicUser(id);
                    return response != null ? Results.Ok(response) : Results.UnprocessableEntity(response);
                }).RequireCors(myCors);

            return app;
        }
    }
}
