using MongoDB.Driver;
using Prestadito.Security.Domain.MainModule.Entities;
using Prestadito.Security.Infrastructure.Data.Constants;

namespace Prestadito.Security.Application.Manager.QueryBuilder.UpdateDefinition
{
    public class SessionUpdateDefinition
    {
        public static UpdateDefinition<SessionEntity> RestartAttemps()
        {
            var update = Builders<SessionEntity>.Update
                .Set(u => u.IntAttempts, 0)
                .Set(u => u.StrComment, ConstantMessages.Sessions.USER_RESTART_ATTEMPS_BY_UNLOCK);

            return update;
        }
    }
}
