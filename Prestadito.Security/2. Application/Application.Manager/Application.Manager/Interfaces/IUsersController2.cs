using Microsoft.AspNetCore.Http;
using Prestadito.Security.Application.Dto.User.CreateUser;
using Prestadito.Security.Application.Dto.User.UpdateUser;

namespace Prestadito.Security.Application.Manager.Interfaces
{
    public interface IUsersController
    {
        ValueTask<IResult> CreateUser(CreateUserRequest dto, string path);
        ValueTask<IResult> GetAllUsers();
        ValueTask<IResult> GetActiveUsers();
        ValueTask<IResult> GetUserById(string id);
        ValueTask<IResult> UpdateUser(UpdateUserRequest dto);
        ValueTask<IResult> DisableUser(string id);
        ValueTask<IResult> DeleteUser(string id);
    }
}
