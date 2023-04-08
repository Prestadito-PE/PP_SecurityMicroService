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

        public static UpdateDefinition<UserEntity> UnlockUserByAttemps()
        {
            var update = Builders<UserEntity>.Update.Set(u => u.BlnLockByAttempts, false);
            return update;
        }

        public static UpdateDefinition<UserEntity> Disable()
        {
            var update = Builders<UserEntity>.Update.Set(u => u.BlnActive, false);
            return update;
        }

        public static UpdateDefinition<UserEntity> UpdateUser(UserEntity entity)
        {
            var update = Builders<UserEntity>.Update
                .Set(u => u.StrEmail, entity.StrEmail)
                .Set(u => u.StrPasswordHash, entity.StrPasswordHash)
                .Set(u => u.StrRolId, entity.StrRolId)
                .Set(u => u.BlnEmailValidated, entity.BlnEmailValidated)
                .Set(u => u.BlnLockByAttempts, entity.BlnLockByAttempts)
                .Set(u => u.BlnCompleteInformation, entity.BlnCompleteInformation)
                .Set(u => u.BlnActive, entity.BlnActive)
                .Set(u => u.DteUpdatedAt, entity.DteUpdatedAt)
                .Set(u => u.StrUpdateUser, entity.StrUpdateUser);
            return update;
        }
    }
}
