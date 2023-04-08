using MongoDB.Driver;
using Prestadito.Security.Application.Manager.QueryBuilder.FilterDefinition;
using Prestadito.Security.Application.Manager.QueryBuilder.SortDefinition;
using Prestadito.Security.Application.Manager.QueryBuilder.UpdateDefinition;
using Prestadito.Security.Domain.MainModule.Entities;

namespace Prestadito.Security.Application.Manager.QueryBuilder
{
    public class SessionQueryBuilder
    {
        public static FilterDefinition<SessionEntity> FindAllSessions()
        {
            var query = SessionFilterDefinition.FindAllSessions();
            return query;
        }

        public static FilterDefinition<SessionEntity> FindSessionById(string sessionId)
        {
            var query = SessionFilterDefinition.FindSessionById(sessionId);
            return query;
        }

        public static FilterDefinition<SessionEntity> FindSessionByUserId(string userId)
        {
            var query = SessionFilterDefinition.FindSessionByUserId(userId);
            return query;
        }

        public static Tuple<FilterDefinition<SessionEntity>, FindOptions<SessionEntity>> FindSessionLastSortByDateLogin(string userId)
        {
            var filterDefinition = SessionFilterDefinition.FindSessionByUserId(userId);
            var findOptions = SessionSortDefinition.SortByDteLoginDesc(1);
            return Tuple.Create(filterDefinition, findOptions);
        }

        public static Tuple<FilterDefinition<SessionEntity>, FindOptions<SessionEntity>, UpdateDefinition<SessionEntity>> RestartAttemps(string userId)
        {
            var filterDefinition = SessionFilterDefinition.FindSessionByUserId(userId);
            var findOptions = SessionSortDefinition.SortByDteLoginDesc(1);
            var updateDefinition = SessionUpdateDefinition.RestartAttemps();
            return Tuple.Create(filterDefinition, findOptions, updateDefinition);
        }
    }
}
