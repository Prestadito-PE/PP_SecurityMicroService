using MongoDB.Driver;
using Prestadito.Security.Domain.MainModule.Entities;

namespace Prestadito.Security.Infrastructure.Data.Interface
{
    public interface ISessionRepository
    {
        ValueTask<SessionEntity> GetSingleAsync(FilterDefinition<SessionEntity> filterDefinition);
        ValueTask<SessionEntity> GetSingleWithOptionsAsync(FilterDefinition<SessionEntity> filterDefinition, FindOptions<SessionEntity, SessionEntity> findOptions);
        ValueTask<IEnumerable<SessionEntity>> GetAsync(FilterDefinition<SessionEntity> filterDefinition);
        ValueTask<IEnumerable<SessionEntity>> GetWithOptionsAsync(FilterDefinition<SessionEntity> filterDefinition, FindOptions<SessionEntity, SessionEntity> findOptions);
        ValueTask InsertOneAsync(SessionEntity entity);
        ValueTask<bool> UpdateOneAsync(FilterDefinition<SessionEntity> filterDefinition, UpdateDefinition<SessionEntity> updateDefinition);
        ValueTask<bool> DeleteOneAsync(FilterDefinition<SessionEntity> filterDefinition);
    }
}
