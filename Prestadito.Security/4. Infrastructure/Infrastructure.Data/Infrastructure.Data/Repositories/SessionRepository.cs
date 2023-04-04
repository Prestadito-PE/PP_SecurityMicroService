using MongoDB.Driver;
using Prestadito.Security.Domain.MainModule.Entities;
using Prestadito.Security.Infrastructure.Data.Interface;
using System.Linq.Expressions;

namespace Prestadito.Security.Infrastructure.Data.Repositories
{
    public class SessionRepository : ISessionRepository
    {
        private readonly IMongoContext _context;

        public SessionRepository(IMongoContext context)
        {
            _context = context;
        }

        public async ValueTask<bool> DeleteOneAsync(Expression<Func<SessionEntity, bool>> filter)
        {
            var result = await _context.Sessions.DeleteOneAsync(filter);
            return result.IsAcknowledged;
        }

        public async ValueTask<List<SessionEntity>> GetAllAsync(Expression<Func<SessionEntity, bool>> filter)
        {
            var result = await _context.Sessions.FindAsync(filter);
            return await result.ToListAsync();
        }

        public async ValueTask<SessionEntity> GetAsync(Expression<Func<SessionEntity, bool>> filter)
        {
            var result = await _context.Sessions.FindAsync(filter);
            return await result.SingleOrDefaultAsync();
        }

        public async ValueTask<List<SessionEntity>> GetFindOptionsAsync(Expression<Func<SessionEntity, bool>> filter, FindOptions<SessionEntity, SessionEntity> findOptions)
        {
            var result = await _context.Sessions.FindAsync(filter, findOptions);
            return await result.ToListAsync();
        }

        public async ValueTask<SessionEntity> GetSingleFilterAndFindOptionsAsync(FilterDefinition<SessionEntity> filterDefinition, FindOptions<SessionEntity, SessionEntity> findOptions)
        {
            var result = await _context.Sessions.FindAsync(filterDefinition, findOptions);
            return await result.SingleOrDefaultAsync();
        }

        public async ValueTask InsertOneAsync(SessionEntity entity)
        {
            await _context.Sessions.InsertOneAsync(entity);
        }

        public async ValueTask<bool> UpdateOneAsync(Expression<Func<SessionEntity, bool>> filter, UpdateDefinition<SessionEntity> updateDefinition)
        {
            var result = await _context.Sessions.UpdateOneAsync(filter, updateDefinition);
            return result.IsAcknowledged;
        }
    }
}
