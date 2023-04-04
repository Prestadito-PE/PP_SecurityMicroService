using MongoDB.Driver;
using Prestadito.Security.Domain.MainModule.Entities;

namespace Prestadito.Security.Infrastructure.Data.Interface
{
    public interface IUserRepository
    {
        ValueTask<UserEntity> GetSingleAsync(FilterDefinition<UserEntity> filterDefinition);
        ValueTask<IEnumerable<UserEntity>> GetAsync(FilterDefinition<UserEntity> filterDefinition);
        ValueTask InsertOneAsync(UserEntity entity);
        ValueTask<bool> UpdateOneAsync(FilterDefinition<UserEntity> filterDefinition, UpdateDefinition<UserEntity> updateDefinition);
        ValueTask<bool> DeleteOneAsync(FilterDefinition<UserEntity> filterDefinition);
    }
}
