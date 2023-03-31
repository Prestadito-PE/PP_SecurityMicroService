using Microsoft.AspNetCore.Http;
using Prestadito.Security.Application.Dto.User.CreateUser;
using Prestadito.Security.Application.Dto.User.DisableUser;
using Prestadito.Security.Application.Dto.User.GetUserById;
using Prestadito.Security.Application.Dto.User.UpdateUser;
using Prestadito.Security.Application.Manager.Interfaces;
using Prestadito.Security.Application.Manager.Mapper;
using Prestadito.Security.Application.Manager.Utilities;
using Prestadito.Security.Domain.MainModule.Entities;
using Prestadito.Security.Infrastructure.Data.Constants;
using Prestadito.Security.Infrastructure.Data.Interface;
using Prestadito.Security.Infrastructure.Data.Utilities;
using System.Linq.Expressions;

namespace Prestadito.Security.Application.Manager.Controller
{
    public class UsersController : IUsersController
    {
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async ValueTask<IResult> CreateUser(CreateUserRequest request, string path)
        {
            ResponseModel<CreateUserResponse> responseModel;

            Expression<Func<UserEntity, bool>> filter = f => f.StrEmail == request.StrEmail;
            var userExist = await _userRepository.GetAsync(filter);
            if (userExist is not null && userExist.Any())
            {
                responseModel = ResponseModel<CreateUserResponse>.GetResponse(ConstantMessages.Errors.Users.EMAIL_ALREADY_EXIST);
                return Results.NotFound(responseModel);
            }

            var entity = new UserEntity
            {
                StrEmail = request.StrEmail,
                StrPasswordHash = CryptoHelper.EncryptAES(request.StrPassword),
                StrRolId = request.StrRolId,
                BlnEmailValidated = false,
                BlnLockByAttempts = false,
                BlnCompleteInformation = false,
                BlnActive = true
            };

            await _userRepository.InsertOneAsync(entity);
            if (string.IsNullOrEmpty(entity.Id))
            {
                responseModel = ResponseModel<CreateUserResponse>.GetResponse(ConstantMessages.Errors.Users.USER_FAILED_TO_CREATE);
                return Results.UnprocessableEntity(responseModel);
            }

            responseModel = ResponseModel<CreateUserResponse>.GetResponse(entity.MapCreateUser());
            return Results.Created($"{path}/{responseModel.Item.StrId}", responseModel);
        }

        public async ValueTask<IResult> GetAllUsers()
        {
            ResponseModel<UserEntity> responseModel;

            Expression<Func<UserEntity, bool>> filter = f => true;
            var entities = await _userRepository.GetAsync(filter);

            responseModel = ResponseModel<UserEntity>.GetResponse(entities.ToList());
            return Results.Json(responseModel);
        }

        public async ValueTask<IResult> GetActiveUsers()
        {
            ResponseModel<UserEntity> responseModel;

            Expression<Func<UserEntity, bool>> filter = f => f.BlnActive;
            var entities = await _userRepository.GetAsync(filter);

            responseModel = ResponseModel<UserEntity>.GetResponse(entities.ToList());
            return Results.Json(responseModel);
        }

        public async ValueTask<IResult> GetUserById(GetUserByIdRequest request)
        {
            ResponseModel<GetUserByIdResponse> responseModel;

            Expression<Func<UserEntity, bool>> filter = f => f.Id == request.StrId;
            var entity = await _userRepository.GetSingleAsync(filter);
            if (entity is null)
            {
                responseModel = ResponseModel<GetUserByIdResponse>.GetResponse(ConstantMessages.Errors.Users.USER_NOT_FOUND);
                return Results.NotFound(responseModel);
            }

            responseModel = ResponseModel<GetUserByIdResponse>.GetResponse(entity.MapGetUserById());
            return Results.Json(responseModel);
        }

        public async ValueTask<IResult> UpdateUser(UpdateUserRequest dto)
        {
            ResponseModel<UserEntity> responseModel;

            Expression<Func<UserEntity, bool>> filter = f => f.Id == dto.Id;
            var entity = await _userRepository.GetSingleAsync(filter);
            if (entity is null)
            {
                responseModel = ResponseModel<UserEntity>.GetResponse(ConstantMessages.Errors.Users.USER_NOT_FOUND);
                return Results.NotFound(responseModel);
            }

            entity.StrDOI = dto.StrDOI;
            entity.StrPasswordHash = CryptoHelper.EncryptAES(dto.StrPassword);
            entity.BlnEmailValidated = true;
            entity.StrRolId = dto.StrRolId;

            var isUserUpdated = await _userRepository.UpdateOneAsync(filter, null);

            if (!isUserUpdated)
            {
                responseModel = ResponseModel<UserEntity>.GetResponse(ConstantMessages.Errors.Users.USER_FAILED_TO_DELETE);
                return Results.UnprocessableEntity(responseModel);
            }

            var userModelItem = new UserEntity
            {
                Id = entity.Id,
                StrDOI = entity.StrDOI,
                StrRolId = entity.StrRolId,
                BlnActive = entity.BlnActive
            };
            responseModel = ResponseModel<UserEntity>.GetResponse(userModelItem);
            return Results.Json(responseModel);
        }

        public async ValueTask<IResult> DisableUser(string id)
        {
            ResponseModel<DisableUserResponse> responseModel;

            Expression<Func<UserEntity, bool>> filter = f => f.Id == id;
            var entity = await _userRepository.GetSingleAsync(filter);
            if (entity is null)
            {
                responseModel = ResponseModel<DisableUserResponse>.GetResponse(ConstantMessages.Errors.Users.USER_NOT_FOUND);
                return Results.NotFound(responseModel);
            }

            entity.BlnActive = false;

            var isUserUpdated = await _userRepository.UpdateOneAsync(filter, null);
            if (!isUserUpdated)
            {
                responseModel = ResponseModel<DisableUserResponse>.GetResponse(ConstantMessages.Errors.Users.USER_FAILED_TO_DISABLE);
                return Results.UnprocessableEntity(responseModel);
            }

            responseModel = ResponseModel<DisableUserResponse>.GetResponse(entity.MapDisableUser());
            return Results.Json(responseModel);
        }

        public async ValueTask<IResult> DeleteUser(DeleteUserRequest request)
        {
            ResponseModel<DeleteUserResponse> responseModel;

            Expression<Func<UserEntity, bool>> filter = f => f.Id == request.StrId;
            var entity = await _userRepository.GetSingleAsync(filter);
            if (entity is null)
            {
                responseModel = ResponseModel<DeleteUserResponse>.GetResponse(ConstantMessages.Errors.Users.USER_NOT_FOUND);
                return Results.NotFound(responseModel);
            }

            var isUserUpdated = await _userRepository.DeleteOneAsync(filter);
            if (!isUserUpdated)
            {
                responseModel = ResponseModel<DeleteUserResponse>.GetResponse(ConstantMessages.Errors.Users.USER_FAILED_TO_DELETE);
                return Results.UnprocessableEntity(responseModel);
            }

            responseModel = ResponseModel<DeleteUserResponse>.GetResponse(entity.MapDeleteUser());
            return Results.Json(responseModel);
        }
    }
}
