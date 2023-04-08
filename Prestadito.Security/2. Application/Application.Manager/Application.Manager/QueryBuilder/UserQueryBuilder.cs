using MongoDB.Driver;
using Prestadito.Security.Application.Manager.QueryBuilder.FilterDefinition;
using Prestadito.Security.Application.Manager.QueryBuilder.UpdateDefinition;
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
            var filterDefinition = UserFilterDefinition.FindUserById(userId);
            var update = UserUpdateDefinition.LockUserByAttempts();

            return Tuple.Create(filterDefinition, update);
        }

        public static Tuple<FilterDefinition<UserEntity>, UpdateDefinition<UserEntity>> UnlockUserByAttemps(string userId)
        {
            var filterDefinition = UserFilterDefinition.FindUserById(userId);
            var updateDefinition = UserUpdateDefinition.UnlockUserByAttemps();

            return Tuple.Create(filterDefinition, updateDefinition);
        }

        public static Tuple<FilterDefinition<UserEntity>, UpdateDefinition<UserEntity>> UpdateUserDisable(string userId)
        {
            var filterDefinition = UserFilterDefinition.FindUserById(userId);
            var updateDefinition = UserUpdateDefinition.Disable();

            return Tuple.Create(filterDefinition, updateDefinition);
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

        public static FilterDefinition<UserEntity> FindUserById(string userId)
        {
            var query = UserFilterDefinition.FindUserById(userId);
            return query;
        }

        public static UpdateDefinition<UserEntity> UpdateUser(UserEntity entity)
        {
            var updateDefinition = UserUpdateDefinition.UpdateUser(entity);
            return updateDefinition;
        }
    }
}
