using MongoDB.Driver;
using Prestadito.Security.Domain.MainModule.Entities;

namespace Prestadito.Security.Infrastructure.Data.Interface
{
    public interface IMongoContext
    {
        IMongoCollection<UserEntity> Users { get; }
        IMongoCollection<SessionEntity> Sessions { get; }
    }
}
