using Microsoft.AspNetCore.Http;
using Prestadito.Security.Application.Dto.User.CreateUser;
using Prestadito.Security.Application.Dto.User.GetUserById;
using Prestadito.Security.Application.Dto.User.UpdateUser;

namespace Prestadito.Security.Application.Manager.Interfaces
{
    public interface IUsersController
    {
        ValueTask<IResult> CreateUser(CreateUserRequest request, string path);
        ValueTask<IResult> GetAllUsers();
        ValueTask<IResult> GetActiveUsers();
        ValueTask<IResult> GetUserById(GetUserByIdRequest request);
        ValueTask<IResult> UpdateUser(UpdateUserRequest request);
        ValueTask<IResult> DisableUser(string id);
        ValueTask<IResult> DeleteUser(DeleteUserRequest request);
    }
}
