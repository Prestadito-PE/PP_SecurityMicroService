namespace Prestadito.Security.Application.Services.Repositories
{
    public class SessionRepository : ISessionRepository
    {
        private readonly IMongoCollection<SessionEntity> collection;

        public SessionRepository(IMongoDatabase database)
        {
            collection = database.GetCollection<SessionEntity>(CollectionsName.Sessions);
        }
        public async ValueTask<List<SessionEntity>> GetSessionsAsync(Expression<Func<SessionEntity, bool>> filter)
        {
            var result = await collection.FindAsync(filter);
            return await result.ToListAsync();
        }
    }
}
