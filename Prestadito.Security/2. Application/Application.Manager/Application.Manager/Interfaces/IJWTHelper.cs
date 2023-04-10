using Prestadito.Security.Application.Dto.Session.Login;
using Prestadito.Security.Domain.MainModule.Entities;

namespace Prestadito.Security.Application.Manager.Interfaces
{
    public interface IJWTHelper
    {
        LoginResponse GenerateToken(UserEntity entity);
    }
}
