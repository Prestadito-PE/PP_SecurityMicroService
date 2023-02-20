namespace Prestadito.Security.Application.Manager.Endpoints
{
    public static class UserEndpoints
    {
        readonly static string collection = "users";
        public static WebApplication UseUserEndpoints(this WebApplication app, string cors, string basePath)
        {
            string path = string.Format("{0}/{1}", basePath, collection);

            app.MapPost(path + "/login",
                async (LoginDTO dto, IUsersController controller) =>
                {
                    return await controller.Login(dto);
                }).RequireCors(cors);

            app.MapPost(path,
                async (IValidator<CreateUserDTO> validator, CreateUserDTO dto, IUsersController controller) =>
                {
                    var validationResult = await validator.ValidateAsync(dto);
                    if (!validationResult.IsValid)
                    {
                        return Results.ValidationProblem(validationResult.ToDictionary());
                    }
                    return await controller.CreateUser(dto, string.Format("~/{0}", path));
                }).RequireCors(cors);

            app.MapGet(path + "/all",
                async (IUsersController controller) =>
                {
                    return await controller.GetAllUsers();
                }).RequireCors(cors);

            app.MapGet(path,
                async (IUsersController controller) =>
                {
                    return await controller.GetActiveUsers();
                }).RequireCors(cors).RequireAuthorization(); ;

            app.MapGet(path + "/{id}",
                async (string id, IUsersController controller) =>
                {
                    return await controller.GetUserById(id);
                }).RequireCors(cors);

            app.MapPut(path,
                async (IValidator<UpdateUserDTO> validator, UpdateUserDTO dto, IUsersController controller) =>
                {
                    var validationResult = await validator.ValidateAsync(dto);
                    if (!validationResult.IsValid)
                    {
                        return Results.ValidationProblem(validationResult.ToDictionary());
                    }
                    return await controller.UpdateUser(dto);
                }).RequireCors(cors);

            app.MapPut(path + "/disable/{id}",
                async (string id, IUsersController controller) =>
                {
                    var response = await controller.DisableUser(id);
                    return response != null ? Results.Ok(response) : Results.UnprocessableEntity(response);
                }).RequireCors(cors);

            app.MapDelete(path + "/delete/{id}",
                async (string id, IUsersController controller) =>
                {
                    var response = await controller.DeleteUser(id);
                    return response != null ? Results.Ok(response) : Results.UnprocessableEntity(response);
                }).RequireCors(cors);

            return app;
        }
    }
}
