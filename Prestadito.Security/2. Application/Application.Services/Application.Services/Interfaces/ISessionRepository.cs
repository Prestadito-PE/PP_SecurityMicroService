namespace Prestadito.Security.Application.Services.Interfaces
{
    public interface ISessionRepository
    {
        ValueTask<List<SessionEntity>> GetSessionsAsync(Expression<Func<SessionEntity, bool>> filter);
    }
}
