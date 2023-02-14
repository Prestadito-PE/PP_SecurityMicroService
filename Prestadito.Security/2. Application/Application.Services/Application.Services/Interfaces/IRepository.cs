namespace Prestadito.Security.Application.Services.Interfaces
{
    public interface IRepository<T>
    {
        ValueTask<List<T>> GetAllAsync();
        ValueTask<T> InsertOneAsync(T entity);
        ValueTask<bool> UpdateOneAsync(T entity);
        ValueTask<bool> DeleteOneAsync(Expression<Func<T, bool>> filter);
        ValueTask<bool> DeleteOneLogicAsync(Expression<Func<User, bool>> filter, User entity);
    }
}
