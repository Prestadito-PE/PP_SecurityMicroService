using Prestadito.Security.Application.Dto.User.CreateUser;
using Prestadito.Security.Application.Dto.User.DisableUser;
using Prestadito.Security.Application.Dto.User.GetUserById;
using Prestadito.Security.Application.Dto.User.UpdateUser;
using Prestadito.Security.Domain.MainModule.Entities;
using Prestadito.Security.Infrastructure.Data.Utilities;

namespace Prestadito.Security.Application.Manager.Mapper
{
    public static class UserMapper
    {
        public static UserEntity MapToEntity(this CreateUserRequest request)
        {
            return new UserEntity
            {
                StrEmail = request.StrEmail,
                StrPasswordHash = CryptoHelper.EncryptAES(request.StrPassword),
                StrRolId = request.StrRolId,
                BlnEmailValidated = false,
                BlnLockByAttempts = false,
                BlnCompleteInformation = false,
                BlnActive = true
            };
        }

        public static GetUserByIdResponse MapGetUserById(this UserEntity entity)
        {
            return new GetUserByIdResponse
            {
                StrId = entity.Id,
                StrRolId = entity.StrRolId,
                BlnEmailValidated = entity.BlnEmailValidated,
                StrEmail = entity.StrEmail,
            };
        }

        public static CreateUserResponse MapCreateUser(this UserEntity entity)
        {
            return new CreateUserResponse
            {
                StrId = entity.Id,
                StrEmail = entity.StrEmail,
                StrRolId = entity.StrRolId,
                BlnEmailValidated = entity.BlnEmailValidated,
                BlnLockByAttempts = entity.BlnLockByAttempts,
                BlnCompleteInformation = entity.BlnCompleteInformation
            };
        }

        public static UpdateUserResponse MapUpdateUser(this UserEntity entity)
        {
            return new UpdateUserResponse
            {
                Id = entity.Id,
                StrEmail = entity.StrEmail,
                StrRolId = entity.StrRolId,
                BlnEmailValidated = entity.BlnEmailValidated,
            };
        }

        public static DisableUserResponse MapDisableUser(this UserEntity entity)
        {
            return new DisableUserResponse
            {
                StrId = entity.Id,
                StrRolId = entity.StrRolId,
                BlnEmailValidated = entity.BlnEmailValidated,
                StrEmail = entity.StrEmail,
            };
        }

        public static DeleteUserResponse MapDeleteUser(this UserEntity entity)
        {
            return new DeleteUserResponse
            {
                StrId = entity.Id,
                StrRolId = entity.StrRolId,
                BlnEmailValidated = entity.BlnEmailValidated,
                StrEmail = entity.StrEmail,
            };
        }
    }
}
