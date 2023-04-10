using MongoDB.Driver;
using Prestadito.Security.Domain.MainModule.Entities;

namespace Prestadito.Security.Application.Manager.QueryBuilder.FilterDefinition
{
    public static class SessionFilterDefinition
    {
        public static FilterDefinition<SessionEntity> FindSessionById(string sessionId)
        {
            var filter = Builders<SessionEntity>.Filter.Eq(s => s.Id, sessionId);
            return filter;
        }

        public static FilterDefinition<SessionEntity> FindSessionByUserId(string userId)
        {
            var filter = Builders<SessionEntity>.Filter.Eq(s => s.StrUserId, userId);
            return filter;
        }

        public static FilterDefinition<SessionEntity> FindAllSessions()
        {
            var filter = Builders<SessionEntity>.Filter.Empty;
            return filter;
        }
    }
}
