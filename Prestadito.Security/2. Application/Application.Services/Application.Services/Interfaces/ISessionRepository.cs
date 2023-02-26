using Prestadito.Security.Domain.MainModule.Entities;
using System.Linq.Expressions;

namespace Prestadito.Security.Application.Services.Interfaces
{
    public interface ISessionRepository
    {
        ValueTask<List<SessionEntity>> GetSessionsAsync(Expression<Func<SessionEntity, bool>> filter);
    }
}
