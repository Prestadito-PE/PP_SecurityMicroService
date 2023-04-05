using MongoDB.Driver;
using Prestadito.Security.Domain.MainModule.Entities;

namespace Prestadito.Security.Application.Manager.QueryBuilder.UpdateDefinition
{
    public class UserUpdateDefinition
    {
        public static UpdateDefinition<UserEntity> LockUserByAttempts()
        {
            var update = Builders<UserEntity>.Update.Set(u => u.BlnLockByAttempts, true);
            return update;
        }
    }
}
