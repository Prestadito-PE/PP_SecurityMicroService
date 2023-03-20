using Prestadito.Security.Application.Dto.Login;
using Prestadito.Security.Domain.MainModule.Entities;

namespace Prestadito.Security.Infrastructure.Data.Interface
{
    public interface IJWTHelper
    {
        LoginResponse GenerateToken(UserEntity entity);
    }
}
