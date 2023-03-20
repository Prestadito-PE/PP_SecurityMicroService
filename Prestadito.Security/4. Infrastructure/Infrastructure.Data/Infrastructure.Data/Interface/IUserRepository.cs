using MongoDB.Driver;
using Prestadito.Security.Domain.MainModule.Entities;
using System.Linq.Expressions;

namespace Prestadito.Security.Infrastructure.Data.Interface
{
    public interface IUserRepository
    {
        ValueTask<UserEntity> GetSingleAsync(Expression<Func<UserEntity, bool>> filter);
        ValueTask<IEnumerable<UserEntity>> GetAsync(Expression<Func<UserEntity, bool>> filter);
        ValueTask InsertOneAsync(UserEntity entity);
        ValueTask<bool> UpdateOneAsync(Expression<Func<UserEntity, bool>> filter, UpdateDefinition<UserEntity> updateDefinition);
        ValueTask<bool> DeleteOneAsync(Expression<Func<UserEntity, bool>> filter);
    }
}
