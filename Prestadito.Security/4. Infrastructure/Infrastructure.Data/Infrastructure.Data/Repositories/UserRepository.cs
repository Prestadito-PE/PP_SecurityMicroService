using MongoDB.Driver;
using Prestadito.Security.Domain.MainModule.Entities;
using Prestadito.Security.Infrastructure.Data.Interface;
using System.Linq.Expressions;

namespace Prestadito.Security.Infrastructure.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoContext _context;

        public UserRepository(IMongoContext context)
        {
            _context = context;
        }

        public async ValueTask<UserEntity> GetSingleAsync(Expression<Func<UserEntity, bool>> filter)
        {
            var result = await _context.Users.FindAsync(filter);
            return await result.SingleOrDefaultAsync();
        }

        public async ValueTask<IEnumerable<UserEntity>> GetAsync(Expression<Func<UserEntity, bool>> filter)
        {
            var result = await _context.Users.FindAsync(filter);
            return result.ToEnumerable();
        }

        public async ValueTask InsertOneAsync(UserEntity entity)
        {
            await _context.Users.InsertOneAsync(entity);
        }

        public async ValueTask<bool> UpdateOneAsync(Expression<Func<UserEntity, bool>> filter, UpdateDefinition<UserEntity> updateDefinition)
        {
            var result = await _context.Users.UpdateOneAsync(filter, updateDefinition);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        public async ValueTask<bool> DeleteOneAsync(Expression<Func<UserEntity, bool>> filter)
        {
            var result = await _context.Users.DeleteOneAsync(filter);
            return result.IsAcknowledged && result.DeletedCount > 0;
        }
    }
}
