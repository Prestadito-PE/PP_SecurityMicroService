using MongoDB.Driver;
using Prestadito.Security.Domain.MainModule.Entities;

namespace Prestadito.Security.Application.Manager.QueryBuilder.FilterDefinition
{
    public class SessionFilterDefinition
    {
        public static FilterDefinition<SessionEntity> FilterSessionByUserId(string userId)
        {
            var filter = Builders<SessionEntity>.Filter.Eq(s => s.StrUserId, userId);
            return filter;
        }
    }
}
