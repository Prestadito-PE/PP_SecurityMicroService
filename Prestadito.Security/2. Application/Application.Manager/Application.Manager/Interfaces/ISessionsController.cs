using Microsoft.AspNetCore.Http;
using Prestadito.Security.Application.Dto.Login;

namespace Prestadito.Security.Application.Manager.Interfaces
{
    public interface ISessionsController
    {
        ValueTask<IResult> Login(LoginRequest request, HttpContext httpContext);
        ValueTask<IResult> GetAllSessions();
        ValueTask<IResult> DeleteSession(string id);
    }
}
