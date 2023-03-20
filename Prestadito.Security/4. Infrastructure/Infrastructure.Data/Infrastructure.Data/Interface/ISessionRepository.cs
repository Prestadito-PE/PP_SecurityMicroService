using MongoDB.Driver;
using Prestadito.Security.Domain.MainModule.Entities;
using System.Linq.Expressions;

namespace Prestadito.Security.Infrastructure.Data.Interface
{
    public interface ISessionRepository : IRepository<SessionEntity>
    {
        ValueTask<List<SessionEntity>> GetFindOptionsAsync(Expression<Func<SessionEntity, bool>> filter, FindOptions<SessionEntity, SessionEntity> findOptions);
        ValueTask<SessionEntity> GetSingleFindOptionsAsync(Expression<Func<SessionEntity, bool>> filter, FindOptions<SessionEntity, SessionEntity> findOptions);
    }
}
