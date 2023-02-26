using Microsoft.AspNetCore.Http;
using Prestadito.Security.Application.Dto.Login;
using Prestadito.Security.Application.Dto.User;

namespace Prestadito.Security.Application.Manager.Interfaces
{
    public interface IUsersController
    {
        ValueTask<IResult> Login(LoginDTO dto);
        ValueTask<IResult> CreateUser(CreateUserDTO dto, string path);
        ValueTask<IResult> GetAllUsers();
        ValueTask<IResult> GetActiveUsers();
        ValueTask<IResult> GetUserById(string id);
        ValueTask<IResult> UpdateUser(UpdateUserDTO dto);
        ValueTask<IResult> DisableUser(string id);
        ValueTask<IResult> DeleteUser(string id);
    }
}
