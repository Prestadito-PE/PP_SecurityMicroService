using MongoDB.Driver;
using System.Linq.Expressions;

namespace Prestadito.Security.Infrastructure.Data.Interface
{
    public interface IRepository<T>
    {
        ValueTask<List<T>> GetAllAsync(Expression<Func<T, bool>> filter);
        ValueTask<T> GetAsync(Expression<Func<T, bool>> filter);
        ValueTask InsertOneAsync(T entity);
        ValueTask<bool> UpdateOneAsync(Expression<Func<T, bool>> filter, UpdateDefinition<T> updateDefinition);
        ValueTask<bool> DeleteOneAsync(Expression<Func<T, bool>> filter);
    }
}
