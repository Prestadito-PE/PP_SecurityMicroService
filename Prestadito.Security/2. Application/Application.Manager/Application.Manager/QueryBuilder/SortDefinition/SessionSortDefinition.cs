using MongoDB.Driver;
using Prestadito.Security.Domain.MainModule.Entities;

namespace Prestadito.Security.Application.Manager.QueryBuilder.SortDefinition
{
    public class SessionSortDefinition
    {
        public static FindOptions<SessionEntity> SortByDteLoginDesc(int limit)
        {
            return new FindOptions<SessionEntity>
            {
                Sort = Builders<SessionEntity>.Sort.Descending(x => x.DteLogin),
                Limit = limit
            };
        }
    }
}
