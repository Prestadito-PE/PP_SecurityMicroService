using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Prestadito.Security.Application.Dto.User.CreateUser;
using Prestadito.Security.Application.Dto.User.DisableUser;
using Prestadito.Security.Application.Dto.User.GetUserById;
using Prestadito.Security.Application.Dto.User.GetUsersActive;
using Prestadito.Security.Application.Dto.User.UpdateUser;
using Prestadito.Security.Application.Manager.Interfaces;
using Prestadito.Security.Application.Manager.QueryBuilder;
using Prestadito.Security.Application.Manager.Utilities;
using Prestadito.Security.Domain.MainModule.Entities;
using Prestadito.Security.Infrastructure.Data.Constants;
using Prestadito.Security.Infrastructure.Data.Interface;
using Prestadito.Security.Infrastructure.Data.Utilities;

namespace Prestadito.Security.Application.Manager.Controller
{
    public class UsersController : IUsersController
    {
        private readonly IUserRepository _userRepository;
        private readonly ISessionRepository _sessionRepository;
        private readonly IMapper _mapper;

        public UsersController(IUserRepository userRepository, ISessionRepository sessionRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _sessionRepository = sessionRepository;
            _mapper = mapper;
        }

        public async ValueTask<IResult> CreateUser(CreateUserRequest request, string path)
        {
            ResponseModel<CreateUserResponse> responseModel;

            var filter = UserQueryBuilder.FindUserByEmail(request.StrEmail);

            var userExist = await _userRepository.GetSingleAsync(filter);
            if (userExist is not null)
            {
                responseModel = ResponseModel<CreateUserResponse>.GetResponse(ConstantMessages.Users.EMAIL_ALREADY_EXIST);
                return Results.NotFound(responseModel);
            }

            var entity = _mapper.Map<UserEntity>(request);

            await _userRepository.InsertOneAsync(entity);
            if (string.IsNullOrEmpty(entity.Id))
            {
                responseModel = ResponseModel<CreateUserResponse>.GetResponse(ConstantMessages.Users.USER_FAILED_TO_CREATE);
                return Results.UnprocessableEntity(responseModel);
            }

            responseModel = ResponseModel<CreateUserResponse>.GetResponse(_mapper.Map<CreateUserResponse>(entity));
            return Results.Created($"{path}/{responseModel.Item.StrId}", responseModel);
        }

        public async ValueTask<IResult> UnlockUserByAttemps(string id)
        {
            ResponseModel<DisableUserResponse> responseModel;

            var (filterDefinitionUser, updateDefinitionUser) = UserQueryBuilder.UnlockUserByAttemps(id);
            var (filterDefinitionSession, findOptionsSession, updateDefinitionSession) = SessionQueryBuilder.RestartAttemps(id);

            var entityUser = await _userRepository.GetSingleAsync(filterDefinitionUser);
            if (entityUser is null)
            {
                responseModel = ResponseModel<DisableUserResponse>.GetResponse(ConstantMessages.Users.USER_NOT_FOUND);
                return Results.NotFound(responseModel);
            }
            entityUser.BlnLockByAttempts = false;

            var entitySession = await _sessionRepository.GetSingleWithOptionsAsync(filterDefinitionSession, findOptionsSession);
            if (entitySession is not null)
            {
                entitySession.IntAttempts = 0;
                await _sessionRepository.UpdateOneAsync(filterDefinitionSession, updateDefinitionSession);
            }

            var isUserUpdated = await _userRepository.UpdateOneAsync(filterDefinitionUser, updateDefinitionUser);
            if (!isUserUpdated)
            {
                responseModel = ResponseModel<DisableUserResponse>.GetResponse(ConstantMessages.Users.USER_FAILED_TO_DISABLE);
                return Results.UnprocessableEntity(responseModel);
            }

            responseModel = ResponseModel<DisableUserResponse>.GetResponse(_mapper.Map<DisableUserResponse>(entityUser));
            return Results.Json(responseModel);
        }

        public async ValueTask<IResult> DisableUser(string id)
        {
            ResponseModel<DisableUserResponse> responseModel;

            var (filterDefinition, updateDefinition) = UserQueryBuilder.UpdateUserDisable(id);
            var entity = await _userRepository.GetSingleAsync(filterDefinition);
            if (entity is null)
            {
                responseModel = ResponseModel<DisableUserResponse>.GetResponse(ConstantMessages.Users.USER_NOT_FOUND);
                return Results.NotFound(responseModel);
            }
            entity.BlnActive = false;

            var isUserUpdated = await _userRepository.UpdateOneAsync(filterDefinition, updateDefinition);
            if (!isUserUpdated)
            {
                responseModel = ResponseModel<DisableUserResponse>.GetResponse(ConstantMessages.Users.USER_FAILED_TO_DISABLE);
                return Results.UnprocessableEntity(responseModel);
            }

            var mapperResponse = _mapper.Map<DisableUserResponse>(entity);
            responseModel = ResponseModel<DisableUserResponse>.GetResponse(mapperResponse);
            return Results.Json(responseModel);
        }

        public async ValueTask<IResult> GetActiveUsers()
        {
            ResponseModel<GetUsersActiveResponse> responseModel;

            var filterDefinition = UserQueryBuilder.FindUsersActive();
            var entities = await _userRepository.GetAsync(filterDefinition);

            var mapperResponse = _mapper.Map<List<GetUsersActiveResponse>>(entities.ToList());
            responseModel = ResponseModel<GetUsersActiveResponse>.GetResponse(mapperResponse);
            return Results.Json(responseModel);
        }

        public async ValueTask<IResult> GetAllUsers()
        {
            ResponseModel<UserEntity> responseModel;

            var filterDefinition = UserQueryBuilder.FindAllUsers();
            var entities = await _userRepository.GetAsync(filterDefinition);

            responseModel = ResponseModel<UserEntity>.GetResponse(entities.ToList());
            return Results.Json(responseModel);
        }

        public async ValueTask<IResult> GetUserById(GetUserByIdRequest request)
        {
            ResponseModel<GetUserByIdResponse> responseModel;

            var filterDefinition = UserQueryBuilder.FindUserById(request.StrId);
            var entity = await _userRepository.GetSingleAsync(filterDefinition);
            if (entity is null)
            {
                responseModel = ResponseModel<GetUserByIdResponse>.GetResponse(ConstantMessages.Users.USER_NOT_FOUND);
                return Results.NotFound(responseModel);
            }

            var mapperResponse = _mapper.Map<GetUserByIdResponse>(entity);
            responseModel = ResponseModel<GetUserByIdResponse>.GetResponse(mapperResponse);
            return Results.Json(responseModel);
        }
        public async ValueTask<IResult> UpdateUser(UpdateUserRequest dto)
        {
            ResponseModel<UpdateUserResponse> responseModel;

            var filterDefinition = UserQueryBuilder.FindUserById(dto.StrId);
            var entity = await _userRepository.GetSingleAsync(filterDefinition);
            if (entity is null)
            {
                responseModel = ResponseModel<UpdateUserResponse>.GetResponse(ConstantMessages.Users.USER_NOT_FOUND);
                return Results.NotFound(responseModel);
            }

            entity.StrEmail = dto.StrEmail;
            entity.StrPasswordHash = CryptoHelper.EncryptAES(dto.StrPassword);
            entity.StrRolId = dto.StrRolId;
            entity.BlnEmailValidated = dto.BlnEmailValidated;
            entity.BlnCompleteInformation = dto.BlnCompleteInformation;
            entity.BlnLockByAttempts = dto.BlnLockByAttempts;
            entity.BlnActive = dto.BlnActive;
            entity.DteUpdatedAt = DateTime.Now;
            entity.StrUpdateUser = ConstantAPI.System.SYSTEM_USER;

            var updateDefinition = UserQueryBuilder.UpdateUser(entity);
            var isUserUpdated = await _userRepository.UpdateOneAsync(filterDefinition, updateDefinition);
            if (!isUserUpdated)
            {
                responseModel = ResponseModel<UpdateUserResponse>.GetResponse(ConstantMessages.Users.USER_FAILED_TO_DELETE);
                return Results.UnprocessableEntity(responseModel);
            }

            var mapperResponse = _mapper.Map<UpdateUserResponse>(entity);
            responseModel = ResponseModel<UpdateUserResponse>.GetResponse(mapperResponse);
            return Results.Json(responseModel);
        }

        public async ValueTask<IResult> DeleteUser(DeleteUserRequest request)
        {
            ResponseModel<DeleteUserResponse> responseModel;

            var filterDefinition = UserQueryBuilder.FindUserById(request.StrId);
            var entity = await _userRepository.GetSingleAsync(filterDefinition);
            if (entity is null)
            {
                responseModel = ResponseModel<DeleteUserResponse>.GetResponse(ConstantMessages.Users.USER_NOT_FOUND);
                return Results.NotFound(responseModel);
            }

            var isUserUpdated = await _userRepository.DeleteOneAsync(filterDefinition);
            if (!isUserUpdated)
            {
                responseModel = ResponseModel<DeleteUserResponse>.GetResponse(ConstantMessages.Users.USER_FAILED_TO_DELETE);
                return Results.UnprocessableEntity(responseModel);
            }

            var mapperResponse = _mapper.Map<DeleteUserResponse>(entity);
            responseModel = ResponseModel<DeleteUserResponse>.GetResponse(mapperResponse);
            return Results.Json(responseModel);
        }
    }
}
