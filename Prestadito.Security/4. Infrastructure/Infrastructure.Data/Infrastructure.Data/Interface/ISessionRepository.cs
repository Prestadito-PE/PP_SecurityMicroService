using MongoDB.Driver;
using Prestadito.Security.Domain.MainModule.Entities;
using System.Linq.Expressions;

namespace Prestadito.Security.Infrastructure.Data.Interface
{
    public interface ISessionRepository
    {
        ValueTask<List<SessionEntity>> GetAllAsync(Expression<Func<SessionEntity, bool>> filter);
        ValueTask<SessionEntity> GetAsync(Expression<Func<SessionEntity, bool>> filter);
        ValueTask InsertOneAsync(SessionEntity entity);
        ValueTask<bool> UpdateOneAsync(Expression<Func<SessionEntity, bool>> filter, UpdateDefinition<SessionEntity> updateDefinition);
        ValueTask<bool> DeleteOneAsync(Expression<Func<SessionEntity, bool>> filter);
        ValueTask<List<SessionEntity>> GetFindOptionsAsync(Expression<Func<SessionEntity, bool>> filter, FindOptions<SessionEntity, SessionEntity> findOptions);
        ValueTask<SessionEntity> GetSingleFilterAndFindOptionsAsync(FilterDefinition<SessionEntity> filterDefinition, FindOptions<SessionEntity, SessionEntity> findOptions);
    }
}
