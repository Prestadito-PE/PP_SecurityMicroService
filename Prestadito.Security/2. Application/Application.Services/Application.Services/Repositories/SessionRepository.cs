using MongoDB.Driver;
using Prestadito.Security.Application.Services.Interfaces;
using Prestadito.Security.Application.Services.Utilities;
using Prestadito.Security.Domain.MainModule.Entities;
using System.Linq.Expressions;

namespace Prestadito.Security.Application.Services.Repositories
{
    public class SessionRepository : ISessionRepository
    {
        private readonly IMongoCollection<SessionEntity> collection;

        public SessionRepository(IMongoDatabase database)
        {
            collection = database.GetCollection<SessionEntity>(CollectionsName.Sessions);
        }

        public async ValueTask<bool> DeleteOneAsync(Expression<Func<SessionEntity, bool>> filter)
        {
            var result = await collection.DeleteOneAsync(filter);
            return result.IsAcknowledged;
        }

        public async ValueTask<List<SessionEntity>> GetAllAsync(Expression<Func<SessionEntity, bool>> filter)
        {
            var result = await collection.FindAsync(filter);
            return await result.ToListAsync();
        }

        public async ValueTask<SessionEntity> GetAsync(Expression<Func<SessionEntity, bool>> filter)
        {
            var result = await collection.FindAsync(filter);
            return await result.SingleOrDefaultAsync();
        }

        public async ValueTask<List<SessionEntity>> GetFindOptionsAsync(Expression<Func<SessionEntity, bool>> filter, FindOptions<SessionEntity, SessionEntity> findOptions)
        {
            var result = await collection.FindAsync(filter, findOptions);
            return await result.ToListAsync();
        }

        public async ValueTask<SessionEntity> GetSingleFindOptionsAsync(Expression<Func<SessionEntity, bool>> filter, FindOptions<SessionEntity, SessionEntity> findOptions)
        {
            var result = await collection.FindAsync(filter, findOptions);
            return await result.SingleOrDefaultAsync();
        }

        public async ValueTask<SessionEntity> InsertOneAsync(SessionEntity entity)
        {
            await collection.InsertOneAsync(entity);
            return entity;
        }

        public async ValueTask<bool> ReplaceOneAsync(SessionEntity entity)
        {
            var result = await collection.ReplaceOneAsync(u => u.Id == entity.Id, entity);
            return result.IsAcknowledged;
        }
    }
}
