using MongoDB.Driver;
using Prestadito.Security.Domain.MainModule.Entities;

namespace Prestadito.Security.Application.Manager.QueryBuilder.FilterDefinition
{
    public static class UserFilterDefinition
    {
        public static FilterDefinition<UserEntity> FindUserById(string userId)
        {
            var filter = Builders<UserEntity>.Filter.Eq(s => s.Id, userId);
            return filter;
        }

        public static FilterDefinition<UserEntity> FilterUserByEmail(string email)
        {
            var filter = Builders<UserEntity>.Filter.Eq(s => s.StrEmail, email);
            return filter;
        }

        public static FilterDefinition<UserEntity> FindAllUsers()
        {
            var filter = Builders<UserEntity>.Filter.Empty;
            return filter;
        }

        public static FilterDefinition<UserEntity> FindUsersActive()
        {
            var filter = Builders<UserEntity>.Filter.Eq(s => s.BlnActive, true);
            return filter;
        }
    }
}
