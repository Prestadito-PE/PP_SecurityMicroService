namespace Prestadito.Security.API.Controller
{
    public class UsersController : IUsersController
    {
        private readonly IUserRepository userRepository;
        public UsersController(IDataService dataService)
        {
            userRepository = dataService.Users;
        }

        public async ValueTask<ResponseModel<UserModel>> CreateUser(CreateUserDTO dto)
        {
            if (dto is null)
            {
                return ResponseModel<UserModel>.GetResponse("Request body is null");
            }

            if (string.IsNullOrWhiteSpace(dto.Username) || string.IsNullOrWhiteSpace(dto.Password) || string.IsNullOrWhiteSpace(dto.RolCode))
            {
                return ResponseModel<UserModel>.GetResponse("Request body is not valid");
            }

            var rol = MockRol.GetRolByCode(dto.RolCode);
            if (rol is null)
            {
                return ResponseModel<UserModel>.GetResponse("Rol not exist");
            }

            var entity = new User
            {
                StrUsername = dto.Username,
                StrPassword = dto.Password,
                ObjRol = rol,
                StrCreateUser = "",
                DteCreatedAt = DateTime.UtcNow,
                BlnActive = true
            };

            var newEntity = await userRepository.InsertOneAsync(entity);
            if (newEntity is null)
            {
                return ResponseModel<UserModel>.GetResponse("Entity not created");
            }

            var userModelItem = new UserModel
            {
                Id = newEntity.Id,
                StrUsername = newEntity.StrUsername,
                ObjRol = rol,
                BlnActive = newEntity.BlnActive
            };
            return ResponseModel<UserModel>.GetResponse(userModelItem);
        }

        public async ValueTask<ResponseModel<UserModel>> GetAllUsers()
        {
            var entities = await userRepository.GetAllAsync();

            var userModelItems = entities.Select(u => new UserModel
            {
                Id = u.Id,
                StrUsername = u.StrUsername,
                ObjRol = u.ObjRol,
                BlnActive = u.BlnActive
            }).ToList();
            return ResponseModel<UserModel>.GetResponse(userModelItems);
        }

        public async ValueTask<ResponseModel<UserModel>> GetActiveUsers()
        {
            Expression<Func<User, bool>> filter = f => f.BlnActive;

            var entities = await userRepository.GetUsersAsync(filter);

            var userModelItems = entities.Select(u => new UserModel
            {
                Id = u.Id,
                StrUsername = u.StrUsername,
                ObjRol = u.ObjRol,
                BlnActive = u.BlnActive
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
                StrUsername = entity.StrUsername,
                ObjRol = entity.ObjRol,
                BlnActive = entity.BlnActive
            };
            return ResponseModel<UserModel>.GetResponse(userModelItem);
        }

        public async ValueTask<ResponseModel<UserModel>> UpdateUser(UpdateUserDTO dto)
        {
            if (dto is null)
            {
                return ResponseModel<UserModel>.GetResponse("Request body is null");
            }

            if (string.IsNullOrWhiteSpace(dto.Id) || string.IsNullOrWhiteSpace(dto.Username) || string.IsNullOrWhiteSpace(dto.Password))
            {
                return ResponseModel<UserModel>.GetResponse("Request body is not valid");
            }

            var entity = await userRepository.GetUserByIdAsync(dto.Id);
            if (entity is null)
            {
                return ResponseModel<UserModel>.GetResponse("User not exist");
            }

            var rol = MockRol.GetRolByCode(dto.RolCode);
            if (rol is null)
            {
                return ResponseModel<UserModel>.GetResponse("Rol not exist");
            }

            entity.StrUsername = dto.Username;
            entity.StrPassword = dto.Password;
            entity.ObjRol = rol;

            var isUserUpdated = await userRepository.UpdateOneAsync(entity);

            if (!isUserUpdated)
            {
                return ResponseModel<UserModel>.GetResponse("User not updated");
            }

            var userModelItem = new UserModel
            {
                Id = entity.Id,
                StrUsername = entity.StrUsername,
                ObjRol = entity.ObjRol,
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
            Expression<Func<User, bool>> filter = f => f.Id == dto.Id;

            var isUserUpdated = await userRepository.DeleteOneAsync(filter);
            if (!isUserUpdated)
            {
                return ResponseModel<UserModel>.GetResponse("User not deleted");
            }

            var userModelItem = new UserModel
            {
                Id = entity.Id,
                StrUsername = entity.StrUsername,
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
            Expression<Func<User, bool>> filter = f => f.Id == id;

            var isUserUpdated = await userRepository.DeleteOneLogicAsync(filter, entity);
            if (!isUserUpdated)
            {
                return ResponseModel<UserModel>.GetResponse("User not deleted");
            }

            var userModelItem = new UserModel
            {
                Id = entity.Id,
                StrUsername = entity.StrUsername,
                ObjRol = entity.ObjRol,
                BlnActive = entity.BlnActive
            };
            return ResponseModel<UserModel>.GetResponse(userModelItem);
        }

    }
}
