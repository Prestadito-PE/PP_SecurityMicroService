namespace Prestadito.Security.Application.Manager.Endpoints
{
    public static class UserEndpoints
    {
        readonly static string path = "/api/security";
        public static WebApplication UseSecurityEndpoints(this WebApplication app)
        {
            string complementPath = "/users";
            app.MapPost(path + complementPath,
                async (CreateUserDTO dto, IUsersController controller) =>
                {
                    var response = await controller.CreateUser(dto);
                    return response != null ? Results.Ok(response) : Results.UnprocessableEntity(response);
                });

            app.MapGet(path + complementPath + "/active",
                async (IUsersController controller) =>
                {
                    var response = await controller.GetActiveUsers();
                    return response != null && response.Items != null ? Results.Ok(response) : Results.UnprocessableEntity(response);
                });

            app.MapGet(path + complementPath + "/{id}",
                async (string id, IUsersController controller) =>
                {
                    var response = await controller.GetUserById(id);
                    return response != null ? Results.Ok(response) : Results.UnprocessableEntity(response);
                });

            app.MapPut(path + complementPath,
                async (UpdateUserDTO dto, IUsersController controller) =>
                {
                    var response = await controller.UpdateUser(dto);
                    return response != null ? Results.Ok(response) : Results.UnprocessableEntity(response);
                });

            app.MapPut(path + complementPath + "/delete/{id}",
                async (string id, IUsersController controller) =>
                {
                    var response = await controller.DeleteLogicUser(id);
                    return response != null ? Results.Ok(response) : Results.UnprocessableEntity(response);
                });

            return app;
        }
    }
}
