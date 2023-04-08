using Microsoft.AspNetCore.Http;
using Prestadito.Security.Application.Dto.Session.Login;
using Prestadito.Security.Application.Dto.User.GetUserById;

namespace Prestadito.Security.Application.Manager.Interfaces
{
    public interface ISessionsController
    {
        ValueTask<IResult> Login(LoginRequest request, HttpContext httpContext);
        ValueTask<IResult> GetAllSessions();
        ValueTask<IResult> DeleteSession(DeleteSessionRequest request);
    }
}
