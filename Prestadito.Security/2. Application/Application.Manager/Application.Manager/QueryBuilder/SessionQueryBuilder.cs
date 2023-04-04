using MongoDB.Driver;
using Prestadito.Security.Application.Manager.QueryBuilder.FilterDefinition;
using Prestadito.Security.Application.Manager.QueryBuilder.SortDefinition;
using Prestadito.Security.Domain.MainModule.Entities;

namespace Prestadito.Security.Application.Manager.QueryBuilder
{
    public class SessionQueryBuilder
    {
        public static Tuple<FilterDefinition<SessionEntity>, FindOptions<SessionEntity>> FindSessionLastSortByDateLogin(string userId)
        {
            var filter = SessionFilterDefinition.FilterSessionByUserId(userId);
            var sort = SessionSortDefinition.SortByDteLoginDesc(1);
            return Tuple.Create(filter, sort);
        }
    }
}
