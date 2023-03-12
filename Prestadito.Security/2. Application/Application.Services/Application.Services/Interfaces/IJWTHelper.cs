using Prestadito.Security.Application.Dto.Login;
using Prestadito.Security.Application.Manager.Models;

namespace Prestadito.Security.Application.Services.Interfaces
{
    public interface IJWTHelper
    {
        LoginResponseDTO GenerateToken(UserModel entity);
    }
}
