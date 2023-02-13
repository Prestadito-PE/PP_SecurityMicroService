namespace Prestadito.Security.Application.Manager.Interfaces
{
    public interface IUsersController
    {
        ValueTask<ResponseModel<UserModel>> CreateUser(CreateUserDTO dto);
        ValueTask<ResponseModel<UserModel>> GetAllUsers();
        ValueTask<ResponseModel<UserModel>> GetActiveUsers();
        ValueTask<ResponseModel<UserModel>> GetUserById(string id);
        ValueTask<ResponseModel<UserModel>> UpdateUser(UpdateUserDTO dto);
        ValueTask<ResponseModel<UserModel>> DeleteUser(DeleteUserDTO dto);
        ValueTask<ResponseModel<UserModel>> DeleteLogicUser(string id);
    }
}
