using Prestadito.Security.Application.Dto.User.CreateUser;
using Prestadito.Security.Application.Dto.User.DisableUser;
using Prestadito.Security.Application.Dto.User.GetUserById;
using Prestadito.Security.Application.Dto.User.UpdateUser;
using Prestadito.Security.Domain.MainModule.Entities;

namespace Prestadito.Security.Application.Manager.Mapper
{
    public static class UserMapper
    {
        public static GetUserByIdResponse MapGetUserById(this UserEntity entity)
        {
            return new GetUserByIdResponse
            {
                StrId = entity.Id,
                StrDOI = entity.StrDOI,
                StrRolId = entity.StrRolId,
                BlnEmailValidated = entity.BlnEmailValidated,
                StrEmail = entity.StrEmail,
                StrStatusId = entity.StrStatusId
            };
        }

        public static CreateUserResponse MapCreateUser(this UserEntity entity)
        {
            return new CreateUserResponse
            {
                StrId = entity.Id,
                StrEmail = entity.StrEmail,
                StrRolId = entity.StrRolId,
                StrDOI = entity.StrDOI,
                BlnEmailValidated = entity.BlnEmailValidated,
                StrStatusId = entity.StrStatusId
            };
        }

        public static UpdateUserResponse MapUpdateUser(this UserEntity entity)
        {
            return new UpdateUserResponse
            {
                Id = entity.Id,
                StrEmail = entity.StrEmail,
                StrRolId = entity.StrRolId,
                StrDOI = entity.StrDOI,
                BlnEmailValidated = entity.BlnEmailValidated,
                StrStatusId = entity.StrStatusId
            };
        }

        public static DisableUserResponse MapDisableUser(this UserEntity entity)
        {
            return new DisableUserResponse
            {
                StrId = entity.Id,
                StrDOI = entity.StrDOI,
                StrRolId = entity.StrRolId,
                BlnEmailValidated = entity.BlnEmailValidated,
                StrEmail = entity.StrEmail,
                StrStatusId = entity.StrStatusId
            };
        }

        public static DeleteUserResponse MapDeleteUser(this UserEntity entity)
        {
            return new DeleteUserResponse
            {
                StrId = entity.Id,
                StrDOI = entity.StrDOI,
                StrRolId = entity.StrRolId,
                BlnEmailValidated = entity.BlnEmailValidated,
                StrEmail = entity.StrEmail,
                StrStatusId = entity.StrStatusId
            };
        }
    }
}
