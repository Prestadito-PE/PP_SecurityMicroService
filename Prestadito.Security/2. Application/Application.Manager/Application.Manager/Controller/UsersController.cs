using Microsoft.AspNetCore.Http;
using Prestadito.Security.Application.Dto.Email;
using Prestadito.Security.Application.Dto.User.CreateUser;
using Prestadito.Security.Application.Dto.User.DisableUser;
using Prestadito.Security.Application.Dto.User.GetUserById;
using Prestadito.Security.Application.Dto.User.UpdateUser;
using Prestadito.Security.Application.Manager.Interfaces;
using Prestadito.Security.Application.Manager.Mapper;
using Prestadito.Security.Application.Manager.QueryBuilder;
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
        private readonly HashService _hashService;

        public UsersController(IUserRepository userRepository, HashService hashService)
        {
            _userRepository = userRepository;
            _hashService = hashService;
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

            var entity = request.MapToEntity();

            await _userRepository.InsertOneAsync(entity);
            if (string.IsNullOrEmpty(entity.Id))
            {
                responseModel = ResponseModel<CreateUserResponse>.GetResponse(ConstantMessages.Users.USER_FAILED_TO_CREATE);
                return Results.UnprocessableEntity(responseModel);
            }

            #region MOVER A CREATE USER
            //Email
            string contrasena = request.StrPassword;
            //request.Contrasena = hashService.Encriptar(usuario.Contrasena);

            List<string> correos = new List<string>();
            correos.Add(request.StrEmail);
            RecuperarClaveEmail message = new RecuperarClaveEmail();

            message.CorreoCliente = request.StrEmail;
            message.NombreCliente = request.StrEmail;
            message.Contrasena = contrasena;
            string templateKey = "templateKey_Create";
            var obj = new EmailData<RecuperarClaveEmail>
            {
                EmailType = 2,
                EmailList = correos,
                Model = message,
                HtmlTemplateName = Constantes.CrearUsuario
            };

            await _hashService.EnviarCorreoAsync(obj, message, templateKey);

            //Fin Email
            #endregion

            responseModel = ResponseModel<CreateUserResponse>.GetResponse(entity.MapCreateUser());
            return Results.Created($"{path}/{responseModel.Item.StrId}", responseModel);
        }

        public async ValueTask<IResult> GetAllUsers()
        {
            ResponseModel<UserEntity> responseModel;

            var filterDefinition = UserQueryBuilder.FindAllUsers();
            var entities = await _userRepository.GetAsync(filterDefinition);

            responseModel = ResponseModel<UserEntity>.GetResponse(entities.ToList());
            return Results.Json(responseModel);
        }

        public async ValueTask<IResult> GetActiveUsers()
        {
            ResponseModel<UserEntity> responseModel;

            var filterDefinition = UserQueryBuilder.FindUsersActive();
            var entities = await _userRepository.GetAsync(filterDefinition);

            responseModel = ResponseModel<UserEntity>.GetResponse(entities.ToList());
            return Results.Json(responseModel);
        }

        public async ValueTask<IResult> GetUserById(GetUserByIdRequest request)
        {
            ResponseModel<GetUserByIdResponse> responseModel;

            var filterDefinition = UserQueryBuilder.FindUserByUserId(request.StrId);
            var entity = await _userRepository.GetSingleAsync(filterDefinition);
            if (entity is null)
            {
                responseModel = ResponseModel<GetUserByIdResponse>.GetResponse(ConstantMessages.Users.USER_NOT_FOUND);
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
                responseModel = ResponseModel<UserEntity>.GetResponse(ConstantMessages.Users.USER_NOT_FOUND);
                return Results.NotFound(responseModel);
            }

            entity.StrPasswordHash = CryptoHelper.EncryptAES(dto.StrPassword);
            entity.BlnEmailValidated = true;
            entity.StrRolId = dto.StrRolId;

            var isUserUpdated = await _userRepository.UpdateOneAsync(filter, null);

            if (!isUserUpdated)
            {
                responseModel = ResponseModel<UserEntity>.GetResponse(ConstantMessages.Users.USER_FAILED_TO_DELETE);
                return Results.UnprocessableEntity(responseModel);
            }

            var userModelItem = new UserEntity
            {
                Id = entity.Id,
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
                responseModel = ResponseModel<DisableUserResponse>.GetResponse(ConstantMessages.Users.USER_NOT_FOUND);
                return Results.NotFound(responseModel);
            }

            entity.BlnActive = false;

            var isUserUpdated = await _userRepository.UpdateOneAsync(filter, null);
            if (!isUserUpdated)
            {
                responseModel = ResponseModel<DisableUserResponse>.GetResponse(ConstantMessages.Users.USER_FAILED_TO_DISABLE);
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
                responseModel = ResponseModel<DeleteUserResponse>.GetResponse(ConstantMessages.Users.USER_NOT_FOUND);
                return Results.NotFound(responseModel);
            }

            var isUserUpdated = await _userRepository.DeleteOneAsync(filter);
            if (!isUserUpdated)
            {
                responseModel = ResponseModel<DeleteUserResponse>.GetResponse(ConstantMessages.Users.USER_FAILED_TO_DELETE);
                return Results.UnprocessableEntity(responseModel);
            }

            responseModel = ResponseModel<DeleteUserResponse>.GetResponse(entity.MapDeleteUser());
            return Results.Json(responseModel);
        }
    }
}
