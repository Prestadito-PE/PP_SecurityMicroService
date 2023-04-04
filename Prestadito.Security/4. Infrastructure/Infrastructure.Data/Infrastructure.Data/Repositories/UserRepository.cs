using MongoDB.Driver;
using Prestadito.Security.Domain.MainModule.Entities;
using Prestadito.Security.Infrastructure.Data.Interface;

namespace Prestadito.Security.Infrastructure.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoContext _context;

        public UserRepository(IMongoContext context)
        {
            _context = context;
        }

        public async ValueTask<UserEntity> GetSingleAsync(FilterDefinition<UserEntity> filterDefinition)
        {
            var result = await _context.Users.FindAsync(filterDefinition);
            return await result.SingleOrDefaultAsync();
        }

        public async ValueTask<IEnumerable<UserEntity>> GetAsync(FilterDefinition<UserEntity> filterDefinition)
        {
            var result = await _context.Users.FindAsync(filterDefinition);
            return result.ToEnumerable();
        }

        public async ValueTask InsertOneAsync(UserEntity entity)
        {
            await _context.Users.InsertOneAsync(entity);
        }

        public async ValueTask<bool> UpdateOneAsync(FilterDefinition<UserEntity> filterDefinition, UpdateDefinition<UserEntity> updateDefinition)
        {
            var result = await _context.Users.UpdateOneAsync(filterDefinition, updateDefinition);
            return result.IsAcknowledged && result.ModifiedCount == 1;
        }

        public async ValueTask<bool> DeleteOneAsync(FilterDefinition<UserEntity> filterDefinition)
        {
            var result = await _context.Users.DeleteOneAsync(filterDefinition);
            return result.IsAcknowledged && result.DeletedCount > 0;
        }
    }
}
