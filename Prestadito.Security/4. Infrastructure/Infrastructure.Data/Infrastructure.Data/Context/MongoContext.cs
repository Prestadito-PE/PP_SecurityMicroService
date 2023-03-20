using MongoDB.Driver;
using Prestadito.Security.Domain.MainModule.Entities;
using Prestadito.Security.Infrastructure.Data.Interface;
using Prestadito.Security.Infrastructure.Data.Utilities;

namespace Prestadito.Security.Infrastructure.Data.Context
{
    public class MongoContext : IMongoContext
    {
        private readonly IMongoDatabase database;

        public MongoContext(ISecurityDBSettings settings)
        {
            var client = new MongoClient(settings.ConnectionURI);
            database = client.GetDatabase(settings.DatabaseName);
        }

        public IMongoCollection<UserEntity> Users
        {
            get
            {
                return database.GetCollection<UserEntity>(CollectionsName.colUsers);
            }
        }

        public IMongoCollection<SessionEntity> Sessions
        {
            get
            {
                return database.GetCollection<SessionEntity>(CollectionsName.colSessions);
            }
        }
    }
}
