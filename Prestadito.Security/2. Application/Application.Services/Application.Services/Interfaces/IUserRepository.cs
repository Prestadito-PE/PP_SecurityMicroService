namespace Prestadito.Security.Application.Services.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        ValueTask<User> GetUserByIdAsync(string id);
        ValueTask<List<User>> GetUsersAsync(Expression<Func<User, bool>> filter);
    }
}
