using MongoDB.Driver;
using Prestadito.Security.Application.Manager.QueryBuilder.FilterDefinition;
using Prestadito.Security.Domain.MainModule.Entities;

namespace Prestadito.Security.Application.Manager.QueryBuilder
{
    public class UserQueryBuilder
    {
        public static FilterDefinition<UserEntity> FindUserByEmail(string email)
        {
            var query = UserFilterDefinition.FilterUserByEmail(email);
            return query;
        }

        public static Tuple<FilterDefinition<UserEntity>, UpdateDefinition<UserEntity>> UpdateUserLockAttemps(string userId)
        {
            var filter = UserFilterDefinition.FilterUserByUserId(userId);
            var update = Builders<UserEntity>.Update
                .Set(u => u.BlnLockByAttempts, true);

            return Tuple.Create(filter, update);
        }

        public static FilterDefinition<UserEntity> FindAllUsers()
        {
            var query = UserFilterDefinition.FindAllUsers();
            return query;
        }

        public static FilterDefinition<UserEntity> FindUsersActive()
        {
            var query = UserFilterDefinition.FindUsersActive();
            return query;
        }

        public static FilterDefinition<UserEntity> FindUserByUserId(string userId)
        {
            var query = UserFilterDefinition.FilterUserByUserId(userId);
            return query;
        }
    }
}
