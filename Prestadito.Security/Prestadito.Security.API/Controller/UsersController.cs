using Prestadito.Security.Application.Dto.Login;
using Prestadito.Security.Application.Dto.User;
using Prestadito.Security.Application.Dto.Util;
using Prestadito.Security.Application.Manager.Interfaces;
using Prestadito.Security.Application.Manager.Models;
using Prestadito.Security.Application.Manager.Utilities;
using Prestadito.Security.Application.Services.Interfaces;
using Prestadito.Security.Application.Services.Utilities;
using Prestadito.Security.Domain.MainModule.Entities;
using System.Linq.Expressions;

namespace Prestadito.Security.API.Controller
{
    public class UsersController : IUsersController
    {
        private readonly IUserRepository userRepository;
        public UsersController(IDataService dataService)
        {
            userRepository = dataService.Users;
        }

        public async ValueTask<IResult> Login(LoginDTO dto)
        {
            ResponseModel<LoginResponseDTO> responseModel;

            var passwordHash = CryptoHelper.EncryptAES(dto.StrPassword);
            Expression<Func<UserEntity, bool>> filter = f => f.StrEmail == dto.StrEmail && f.StrPasswordHash == passwordHash;

            var entity = await userRepository.GetAsync(filter);

            if (entity is null)
            {
                responseModel = ResponseModel<LoginResponseDTO>.GetResponse("User not exist");
                return Results.NotFound(responseModel);
            }

            UserModel userMap = new()
            {
                Id = entity.Id,
                StrDOI = entity.StrDOI,
                ObjRol = entity.ObjRol,
                BlnRegisterComplete = entity.BlnRegisterComplete,
                StrEmail = entity.StrEmail,
                ObjStatus = entity.ObjStatus
            };

            LoginResponseDTO loginResponseDTO = JWT.GenerateToken(userMap);
            responseModel = ResponseModel<LoginResponseDTO>.GetResponse(loginResponseDTO);
            return Results.Json(responseModel);
        }

        public async ValueTask<IResult> CreateUser(CreateUserDTO dto, string path)
        {
            ResponseModel<UserModel> responseModel;

            Expression<Func<UserEntity, bool>> filter = f => f.StrEmail == dto.StrEmail;
            var userExist = await userRepository.GetAllAsync(filter);
            if (userExist is not null && userExist.Count > 0)
            {
                responseModel = ResponseModel<UserModel>.GetResponse($"Email is already exist");
                return Results.NotFound(responseModel);
            }

            var rol = Mocks.GetRolByCode(dto.StrRolCode);
            if (rol is null)
            {
                responseModel = ResponseModel<UserModel>.GetResponse("Rol not exist");
                return Results.NotFound(responseModel);
            }

            var userStatus = Mocks.GetUserStatus("0");
            if (userStatus is null)
            {
                responseModel = ResponseModel<UserModel>.GetResponse("UserStatus not exist");
                return Results.NotFound(responseModel);
            }

            var passwordHash = CryptoHelper.EncryptAES(dto.StrPassword);
            var entity = new UserEntity
            {
                StrEmail = dto.StrEmail,
                StrPasswordHash = passwordHash,
                ObjRol = rol,
                BlnRegisterComplete = false,
                ObjStatus = userStatus,
                DteCreatedAt = DateTime.UtcNow,
                BlnActive = true
            };

            var newUser = await userRepository.InsertOneAsync(entity);
            if (newUser is null)
            {
                responseModel = ResponseModel<UserModel>.GetResponse("Entity not created");
                return Results.UnprocessableEntity(responseModel);
            }

            var userModelItem = new UserModel
            {
                Id = newUser.Id,
                StrDOI = newUser.StrDOI,
                ObjRol = rol,
                BlnActive = newUser.BlnActive
            };
            responseModel = ResponseModel<UserModel>.GetResponse(userModelItem);
            return Results.Created(string.Format("{0}/{1}", path, responseModel.Item.Id), responseModel);
        }

        public async ValueTask<IResult> GetAllUsers()
        {
            ResponseModel<UserModel> responseModel;

            Expression<Func<UserEntity, bool>> filter = f => true;
            var entities = await userRepository.GetAllAsync(filter);

            var userModelItems = entities.Select(u => new UserModel
            {
                Id = u.Id,
                StrDOI = u.StrDOI,
                ObjRol = u.ObjRol,
                BlnRegisterComplete = u.BlnRegisterComplete,
                StrEmail = u.StrEmail,
                ObjStatus = u.ObjStatus,
                BlnActive = u.BlnActive
            }).ToList();

            responseModel = ResponseModel<UserModel>.GetResponse(userModelItems);
            return Results.Json(responseModel);
        }

        public async ValueTask<IResult> GetActiveUsers()
        {
            ResponseModel<UserModel> responseModel;

            Expression<Func<UserEntity, bool>> filter = f => f.BlnActive;
            var entities = await userRepository.GetAllAsync(filter);

            var userModelItems = entities.Select(u => new UserModel
            {
                Id = u.Id,
                StrDOI = u.StrDOI,
                ObjRol = u.ObjRol,
                BlnRegisterComplete = u.BlnRegisterComplete,
                StrEmail = u.StrEmail,
                ObjStatus = u.ObjStatus,
                BlnActive = u.BlnActive
            }).ToList();

            responseModel = ResponseModel<UserModel>.GetResponse(userModelItems);
            return Results.Json(responseModel);
        }

        public async ValueTask<IResult> GetUserById(string id)
        {
            ResponseModel<UserModel> responseModel;

            if (string.IsNullOrWhiteSpace(id))
            {
                responseModel = ResponseModel<UserModel>.GetResponse("Id is empty");
                return Results.BadRequest(responseModel);
            }

            Expression<Func<UserEntity, bool>> filter = f => f.Id == id;
            var entity = await userRepository.GetAsync(filter);
            if (entity is null)
            {
                responseModel = ResponseModel<UserModel>.GetResponse("User not found");
                return Results.NotFound(responseModel);
            }

            var userModelItem = new UserModel
            {
                Id = entity.Id,
                StrDOI = entity.StrDOI,
                ObjRol = entity.ObjRol,
                BlnRegisterComplete = entity.BlnRegisterComplete,
                StrEmail = entity.StrEmail,
                ObjStatus = entity.ObjStatus,
                BlnActive = entity.BlnActive
            };

            responseModel = ResponseModel<UserModel>.GetResponse(userModelItem);
            return Results.Json(responseModel);
        }

        public async ValueTask<IResult> UpdateUser(UpdateUserDTO dto)
        {
            ResponseModel<UserModel> responseModel;

            Expression<Func<UserEntity, bool>> filter = f => f.Id == dto.Id;
            var entity = await userRepository.GetAsync(filter);
            if (entity is null)
            {
                responseModel = ResponseModel<UserModel>.GetResponse("User not exist");
                return Results.NotFound(responseModel);
            }

            var rol = Mocks.GetRolByCode(dto.StrRolCode);
            if (rol is null)
            {
                responseModel = ResponseModel<UserModel>.GetResponse("Rol not exist");
                return Results.NotFound(responseModel);
            }

            var userStatus = Mocks.GetUserStatus("0");
            if (userStatus is null)
            {
                responseModel = ResponseModel<UserModel>.GetResponse("UserStatus not exist");
                return Results.NotFound(responseModel);
            }

            entity.StrDOI = dto.StrDOI;
            entity.StrPasswordHash = Hash256.Encrypt(dto.StrPassword);
            entity.BlnRegisterComplete = true;
            entity.ObjStatus = userStatus;
            entity.ObjRol = rol;

            var isUserUpdated = await userRepository.UpdateOneAsync(entity);

            if (!isUserUpdated)
            {
                responseModel = ResponseModel<UserModel>.GetResponse("User not updated");
                return Results.UnprocessableEntity(responseModel);
            }

            var userModelItem = new UserModel
            {
                Id = entity.Id,
                StrDOI = entity.StrDOI,
                ObjRol = rol,
                BlnActive = entity.BlnActive
            };
            responseModel = ResponseModel<UserModel>.GetResponse(userModelItem);
            return Results.Json(responseModel);
        }

        public async ValueTask<IResult> DisableUser(string id)
        {
            ResponseModel<UserModel> responseModel;

            Expression<Func<UserEntity, bool>> filter = f => f.Id == id;
            var entity = await userRepository.GetAsync(filter);
            if (entity is null)
            {
                responseModel = ResponseModel<UserModel>.GetResponse("User not exist");
                return Results.NotFound(responseModel);
            }

            entity.BlnActive = false;
            var isUserUpdated = await userRepository.UpdateOneAsync(entity);
            if (!isUserUpdated)
            {
                responseModel = ResponseModel<UserModel>.GetResponse("User not deleted");
                return Results.UnprocessableEntity(responseModel);
            }

            var userModelItem = new UserModel
            {
                Id = entity.Id,
                StrDOI = entity.StrDOI,
                ObjRol = entity.ObjRol,
                BlnActive = entity.BlnActive
            };
            responseModel = ResponseModel<UserModel>.GetResponse(userModelItem);
            return Results.Json(responseModel);
        }

        public async ValueTask<IResult> DeleteUser(string id)
        {
            ResponseModel<UserModel> responseModel;

            Expression<Func<UserEntity, bool>> filter = f => f.Id == id;
            var entity = await userRepository.GetAsync(filter);
            if (entity is null)
            {
                responseModel = ResponseModel<UserModel>.GetResponse("User not exist");
                return Results.NotFound(responseModel);
            }

            var isUserUpdated = await userRepository.DeleteOneAsync(filter);
            if (!isUserUpdated)
            {
                responseModel = ResponseModel<UserModel>.GetResponse("User not deleted");
                return Results.UnprocessableEntity(responseModel);
            }

            var userModelItem = new UserModel
            {
                Id = entity.Id,
                StrDOI = entity.StrDOI,
                ObjRol = entity.ObjRol,
                BlnActive = entity.BlnActive
            };
            responseModel = ResponseModel<UserModel>.GetResponse(userModelItem);
            return Results.Json(responseModel);
        }
    }
}
