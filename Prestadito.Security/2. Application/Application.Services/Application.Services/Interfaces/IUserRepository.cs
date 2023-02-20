using Prestadito.Security.Application.Dto.Login;
using Prestadito.Security.Application.Dto.User;

namespace Prestadito.Security.Application.Services.Interfaces
{
    public interface IUserRepository : IRepository<UserEntity>
    {
        ValueTask<UserEntity> GetUserByIdAsync(string id);
        ValueTask<List<UserEntity>> GetUsersAsync(Expression<Func<UserEntity, bool>> filter);

        ValueTask<LoginResponseDTO> GetLoginCredentials(LoginDTO dTO);

        ValueTask<LoginResponseDTO> GenerateToken(UserDTO entity);
    }
}
