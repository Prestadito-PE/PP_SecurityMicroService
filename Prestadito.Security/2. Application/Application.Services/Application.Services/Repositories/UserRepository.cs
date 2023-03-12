using MongoDB.Driver;
using Prestadito.Security.Application.Services.Interfaces;
using Prestadito.Security.Application.Services.Utilities;
using Prestadito.Security.Domain.MainModule.Entities;
using System.Linq.Expressions;

namespace Prestadito.Security.Application.Services.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<UserEntity> collection;

        public UserRepository(IMongoDatabase database)
        {
            collection = database.GetCollection<UserEntity>(CollectionsName.Users);
        }

        public async ValueTask<bool> DeleteOneAsync(Expression<Func<UserEntity, bool>> filter)
        {
            var result = await collection.DeleteOneAsync(filter);
            return result.IsAcknowledged;
        }

        public async ValueTask<List<UserEntity>> GetAllAsync(Expression<Func<UserEntity, bool>> filter)
        {
            var result = await collection.FindAsync(filter);
            return await result.ToListAsync();
        }

        public async ValueTask<UserEntity> GetAsync(Expression<Func<UserEntity, bool>> filter)
        {
            var result = await collection.FindAsync(filter);
            return await result.SingleOrDefaultAsync();
        }

        public async ValueTask<UserEntity> InsertOneAsync(UserEntity entity)
        {
            await collection.InsertOneAsync(entity);
            return entity;
        }

        public async ValueTask<bool> ReplaceOneAsync(UserEntity entity)
        {
            var result = await collection.ReplaceOneAsync(u => u.Id == entity.Id, entity);
            return result.IsAcknowledged;
        }
    }
}
