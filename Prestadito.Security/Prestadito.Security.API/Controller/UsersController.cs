using Microsoft.AspNetCore.Mvc;
using Prestadito.Security.Application.Dto.Login;
using Prestadito.Security.Application.Dto.Util;
using Prestadito.Security.Application.Services.Repositories;

namespace Prestadito.Security.API.Controller
{
    public class UsersController : IUsersController
    {
        private readonly IUserRepository userRepository;
        public UsersController(IDataService dataService)
        {
            userRepository = dataService.Users;
 
        }

        public async ValueTask<ResponseModel<LoginResponseDTO>> UserAuthentication(LoginDTO dto)
        {
            dto.StrPasswordHash = CryptoHelper.EncryptAES(dto.StrPasswordHash);
         
            LoginResponseDTO entities = await userRepository.GetLoginCredentials(dto);

            return ResponseModel<LoginResponseDTO>.GetResponse(entities);
        }

        public async ValueTask<ResponseModel<UserModel>> CreateUser(CreateUserDTO dto)
        {
            Expression<Func<UserEntity, bool>> filter = f => f.StrDOI == dto.StrDOI;
            var userExist = await userRepository.GetUsersAsync(filter);
            if (userExist is not null && userExist.Count > 0)
            {
                return ResponseModel<UserModel>.GetResponse("DOI is already exist");
            }

            var rol = Mocks.GetRolByCode(dto.StrRolCode);
            if (rol is null)
            {
                return ResponseModel<UserModel>.GetResponse("Rol not exist");
            }

            var userStatus = Mocks.GetUserStatus("0");
            if (userStatus is null)
            {
                return ResponseModel<UserModel>.GetResponse("UserStatus not exist");
            }

            var entity = new UserEntity
            {
                StrDOI = dto.StrDOI,
                StrPasswordHash = Hash256.Encrypt(dto.StrPassword),
                ObjRol = rol,
                BlnRegisterComplete = false,
                ObjStatus = userStatus,
                DteCreatedAt = DateTime.UtcNow,
                BlnActive = true
            };

            var newUser = await userRepository.InsertOneAsync(entity);
            if (newUser is null)
            {
                return ResponseModel<UserModel>.GetResponse("Entity not created");
            }

            var userModelItem = new UserModel
            {
                Id = newUser.Id,
                StrDOI = newUser.StrDOI,
                ObjRol = rol,
                BlnActive = newUser.BlnActive
            };
            return ResponseModel<UserModel>.GetResponse(userModelItem);
        }

        public async ValueTask<ResponseModel<UserModel>> GetAllUsers()
        {
            var entities = await userRepository.GetAllAsync();

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
            return ResponseModel<UserModel>.GetResponse(userModelItems);
        }

        public async ValueTask<ResponseModel<UserModel>> GetActiveUsers()
        {
            Expression<Func<UserEntity, bool>> filter = f => f.BlnActive;
            var entities = await userRepository.GetUsersAsync(filter);

            var userModelItems = entities.Select(u => new UserModel
            {
                Id = u.Id,
                StrDOI = u.StrDOI,
                ObjRol = u.ObjRol,
                BlnRegisterComplete = u.BlnRegisterComplete,
                StrEmail = u.StrEmail,
                ObjStatus = u.ObjStatus
            }).ToList();
            return ResponseModel<UserModel>.GetResponse(userModelItems);
        }

        public async ValueTask<ResponseModel<UserModel>> GetUserById(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return ResponseModel<UserModel>.GetResponse("Id is empty");
            }

            var entity = await userRepository.GetUserByIdAsync(id);
            if (entity is null)
            {
                return ResponseModel<UserModel>.GetResponse("User not found");
            }

            var userModelItem = new UserModel
            {
                Id = entity.Id,
                StrDOI = entity.StrDOI,
                ObjRol = entity.ObjRol,
                BlnRegisterComplete = entity.BlnRegisterComplete,
                StrEmail = entity.StrEmail,
                ObjStatus = entity.ObjStatus
            };
            return ResponseModel<UserModel>.GetResponse(userModelItem);
        }

        public async ValueTask<ResponseModel<UserModel>> UpdateUser(UpdateUserDTO dto)
        {
            if (dto is null)
            {
                return ResponseModel<UserModel>.GetResponse("Request body is null");
            }

            if (string.IsNullOrWhiteSpace(dto.Id) || string.IsNullOrWhiteSpace(dto.StrDOI) || string.IsNullOrWhiteSpace(dto.StrPassword))
            {
                return ResponseModel<UserModel>.GetResponse("Request body is not valid");
            }

            var entity = await userRepository.GetUserByIdAsync(dto.Id);
            if (entity is null)
            {
                return ResponseModel<UserModel>.GetResponse("User not exist");
            }

            var rol = Mocks.GetRolByCode(dto.StrRolCode);
            if (rol is null)
            {
                return ResponseModel<UserModel>.GetResponse("Rol not exist");
            }

            var userStatus = Mocks.GetUserStatus("0");
            if (userStatus is null)
            {
                return ResponseModel<UserModel>.GetResponse("UserStatus not exist");
            }

            entity.StrDOI = dto.StrDOI;
            entity.StrPasswordHash = Hash256.Encrypt(dto.StrPassword);
            entity.BlnRegisterComplete = true;
            entity.ObjStatus = userStatus;
            entity.ObjRol = rol;

            var isUserUpdated = await userRepository.UpdateOneAsync(entity);

            if (!isUserUpdated)
            {
                return ResponseModel<UserModel>.GetResponse("User not updated");
            }

            var userModelItem = new UserModel
            {
                Id = entity.Id,
                StrDOI = entity.StrDOI,
                ObjRol = rol,
                BlnActive = entity.BlnActive
            };
            return ResponseModel<UserModel>.GetResponse(userModelItem);
        }

        public async ValueTask<ResponseModel<UserModel>> DeleteUser(DeleteUserDTO dto)
        {
            if (dto is null)
            {
                return ResponseModel<UserModel>.GetResponse("Request body is null");
            }

            if (string.IsNullOrWhiteSpace(dto.Id))
            {
                return ResponseModel<UserModel>.GetResponse("Request body is not valid");
            }

            var entity = await userRepository.GetUserByIdAsync(dto.Id);
            if (entity is null)
            {
                return ResponseModel<UserModel>.GetResponse("User not exist");
            }

            entity.BlnActive = false;
            Expression<Func<UserEntity, bool>> filter = f => f.Id == dto.Id;

            var isUserUpdated = await userRepository.DeleteOneAsync(filter);
            if (!isUserUpdated)
            {
                return ResponseModel<UserModel>.GetResponse("User not deleted");
            }

            var userModelItem = new UserModel
            {
                Id = entity.Id,
                StrDOI = entity.StrDOI,
                ObjRol = entity.ObjRol,
                BlnActive = entity.BlnActive
            };
            return ResponseModel<UserModel>.GetResponse(userModelItem);
        }

        public async ValueTask<ResponseModel<UserModel>> DeleteLogicUser(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return ResponseModel<UserModel>.GetResponse("Request param is not valid");
            }

            var entity = await userRepository.GetUserByIdAsync(id);
            if (entity is null)
            {
                return ResponseModel<UserModel>.GetResponse("User not exist");
            }

            entity.BlnActive = false;
            Expression<Func<UserEntity, bool>> filter = f => f.Id == id;

            var isUserUpdated = await userRepository.DeleteOneLogicAsync(filter, entity);
            if (!isUserUpdated)
            {
                return ResponseModel<UserModel>.GetResponse("User not deleted");
            }

            var userModelItem = new UserModel
            {
                Id = entity.Id,
                StrDOI = entity.StrDOI,
                ObjRol = entity.ObjRol,
                BlnActive = entity.BlnActive
            };
            return ResponseModel<UserModel>.GetResponse(userModelItem);
        }

        
    }
}
