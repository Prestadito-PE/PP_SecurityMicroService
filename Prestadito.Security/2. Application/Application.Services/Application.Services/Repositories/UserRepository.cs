﻿namespace Prestadito.Security.Application.Services.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<UserEntity> collection;

        public UserRepository(IMongoDatabase database)
        {
            collection = database.GetCollection<UserEntity>(CollectionsName.Users);
        }

        public async ValueTask<List<UserEntity>> GetAllAsync()
        {
            return await collection.AsQueryable().ToListAsync();
        }

        public async ValueTask<List<UserEntity>> GetUsersAsync(Expression<Func<UserEntity, bool>> filter)
        {
            var result = await collection.FindAsync(filter);
            return await result.ToListAsync();
        }

        public async ValueTask<UserEntity> GetUserByIdAsync(string id)
        {
            var result = await collection.FindAsync(u => u.Id == id);
            return await result.SingleOrDefaultAsync();
        }

        public async ValueTask<UserEntity> InsertOneAsync(UserEntity entity)
        {
            await collection.InsertOneAsync(entity);
            return entity;
        }

        public async ValueTask<bool> UpdateOneAsync(UserEntity entity)
        {
            var result = await collection.ReplaceOneAsync(u => u.Id == entity.Id, entity);
            return result.IsAcknowledged;
        }

        public async ValueTask<bool> DeleteOneAsync(Expression<Func<UserEntity, bool>> filter)
        {
            var result = await collection.DeleteOneAsync(filter);
            return result.IsAcknowledged;
        }

        public async ValueTask<bool> DeleteOneLogicAsync(Expression<Func<UserEntity, bool>> filter, UserEntity entity)
        {
            var result = await collection.ReplaceOneAsync(filter, entity);
            return result.IsAcknowledged;
        }
    }
}
