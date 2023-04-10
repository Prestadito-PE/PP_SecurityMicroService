using MongoDB.Driver;
using Prestadito.Security.Domain.MainModule.Entities;
using Prestadito.Security.Infrastructure.Data.Interface;

namespace Prestadito.Security.Infrastructure.Data.Repositories
{
    public class SessionRepository : ISessionRepository
    {
        private readonly IMongoContext _context;

        public SessionRepository(IMongoContext context)
        {
            _context = context;
        }

        public async ValueTask<SessionEntity> GetSingleAsync(FilterDefinition<SessionEntity> filterDefinition)
        {
            var result = await _context.Sessions.FindAsync(filterDefinition);
            return await result.SingleOrDefaultAsync();
        }

        public async ValueTask<SessionEntity> GetSingleWithOptionsAsync(FilterDefinition<SessionEntity> filterDefinition, FindOptions<SessionEntity, SessionEntity> findOptions)
        {
            var result = await _context.Sessions.FindAsync(filterDefinition, findOptions);
            return await result.SingleOrDefaultAsync();
        }

        public async ValueTask<IEnumerable<SessionEntity>> GetAsync(FilterDefinition<SessionEntity> filterDefinition)
        {
            var result = await _context.Sessions.FindAsync(filterDefinition);
            return result.ToEnumerable();
        }

        public async ValueTask<IEnumerable<SessionEntity>> GetWithOptionsAsync(FilterDefinition<SessionEntity> filterDefinition, FindOptions<SessionEntity, SessionEntity> findOptions)
        {
            var result = await _context.Sessions.FindAsync(filterDefinition, findOptions);
            return result.ToEnumerable();
        }

        public async ValueTask InsertOneAsync(SessionEntity entity)
        {
            await _context.Sessions.InsertOneAsync(entity);
        }

        public async ValueTask<bool> UpdateOneAsync(FilterDefinition<SessionEntity> filterDefinition, UpdateDefinition<SessionEntity> updateDefinition)
        {
            var result = await _context.Sessions.UpdateOneAsync(filterDefinition, updateDefinition);
            return result.IsAcknowledged;
        }

        public async ValueTask<bool> DeleteOneAsync(FilterDefinition<SessionEntity> filterDefinition)
        {
            var result = await _context.Sessions.DeleteOneAsync(filterDefinition);
            return result.IsAcknowledged;
        }
    }
}
