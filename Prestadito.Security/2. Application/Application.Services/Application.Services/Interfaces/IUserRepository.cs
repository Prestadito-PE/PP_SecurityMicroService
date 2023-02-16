namespace Prestadito.Security.Application.Services.Interfaces
{
    public interface IUserRepository : IRepository<UserEntity>
    {
        ValueTask<UserEntity> GetUserByIdAsync(string id);
        ValueTask<List<UserEntity>> GetUsersAsync(Expression<Func<UserEntity, bool>> filter);
    }
}
