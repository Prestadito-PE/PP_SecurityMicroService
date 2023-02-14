namespace Prestadito.Security.Application.Services.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> collection;

        public UserRepository(IMongoDatabase database)
        {
            collection = database.GetCollection<User>(CollectionsName.Users);
        }

        public async ValueTask<List<User>> GetAllAsync()
        {
            return await collection.AsQueryable().ToListAsync();
        }

        public async ValueTask<List<User>> GetUsersAsync(Expression<Func<User, bool>> filter)
        {
            var result = await collection.FindAsync(filter);
            return await result.ToListAsync();
        }

        public async ValueTask<User> GetUserByIdAsync(string id)
        {
            var result = await collection.FindAsync(u => u.Id == id);
            return await result.SingleOrDefaultAsync();
        }

        public async ValueTask<User> InsertOneAsync(User entity)
        {
            await collection.InsertOneAsync(entity);
            return entity;
        }

        public async ValueTask<bool> UpdateOneAsync(User entity)
        {
            var result = await collection.ReplaceOneAsync(u => u.Id == entity.Id, entity);
            return result.IsAcknowledged;
        }

        public async ValueTask<bool> DeleteOneAsync(Expression<Func<User, bool>> filter)
        {
            var result = await collection.DeleteOneAsync(filter);
            return result.IsAcknowledged;
        }

        public async ValueTask<bool> DeleteOneLogicAsync(Expression<Func<User, bool>> filter, User entity)
        {
            var result = await collection.ReplaceOneAsync(filter, entity);
            return result.IsAcknowledged;
        }
    }
}
