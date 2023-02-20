using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using Prestadito.Security.Application.Dto.Login;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Prestadito.Security.Application.Dto.User;
using MongoDB.Bson;
using Prestadito.Security.Application.Manager.Models;

namespace Prestadito.Security.Application.Services.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<UserEntity> collection;

        public UserRepository(IMongoDatabase database)
        {
            collection = database.GetCollection<UserEntity>(CollectionsName.Users);
        }

        public async ValueTask<LoginResponseDTO> GetLoginCredentials(LoginDTO dTO)
        {
            var result = await collection.FindAsync(u => u.StrEmail == dTO.StrEmail &&  u.StrPasswordHash == dTO.StrPasswordHash);

            var userModelItems = result.ToList().Select(u => new UserModel
            {
                Id = u.Id,
                StrDOI = u.StrDOI,
                ObjRol = u.ObjRol,
                BlnRegisterComplete = u.BlnRegisterComplete,
                StrEmail = u.StrEmail,
                ObjStatus = u.ObjStatus
            }).ToList();

            UserDTO userMap = new UserDTO
            {
                Id = userModelItems[0].Id,
                StrDOI = userModelItems[0].StrDOI,
                ObjRol = userModelItems[0].ObjRol,
                BlnRegisterComplete = userModelItems[0].BlnRegisterComplete,
                StrEmail = userModelItems[0].StrEmail,
                ObjStatus = userModelItems[0].ObjStatus
            };

            LoginResponseDTO response = await GenerateToken(userMap);

            return response;
        }

        public async ValueTask<List<UserEntity>> GetAllAsync()
        {
            return await collection.AsQueryable().ToListAsync();
        }

        public async ValueTask<List<UserEntity>> GetUsersAsync(Expression<Func<UserEntity, bool>> filter)
        {
            var result = await collection.FindAsync(filter);
            return await result.ToListAsync();
        }

        public async ValueTask<UserEntity> GetUserByIdAsync(string id)
        {
            var result = await collection.FindAsync(u => u.Id == id);
            return await result.SingleOrDefaultAsync();
        }

        public async ValueTask<UserEntity> InsertOneAsync(UserEntity entity)
        {
            await collection.InsertOneAsync(entity);
            return entity;
        }

        public async ValueTask<bool> UpdateOneAsync(UserEntity entity)
        {
            var result = await collection.ReplaceOneAsync(u => u.Id == entity.Id, entity);
            return result.IsAcknowledged;
        }

        public async ValueTask<bool> DeleteOneAsync(Expression<Func<UserEntity, bool>> filter)
        {
            var result = await collection.DeleteOneAsync(filter);
            return result.IsAcknowledged;
        }

        public async ValueTask<bool> DeleteOneLogicAsync(Expression<Func<UserEntity, bool>> filter, UserEntity entity)
        {
            var result = await collection.ReplaceOneAsync(filter, entity);
            return result.IsAcknowledged;
        }

        public async ValueTask<LoginResponseDTO> GenerateToken(UserDTO entity)
        {
            LoginResponseDTO response = null;
            //Header 
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("8yBEHrPo5rut8alxAWnGd2nvZr4u7xeThWm2Z00q4K2bPeShVm"));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var header = new JwtHeader(signingCredentials);

            //add Claims
            var claims = new[]
            {
                new Claim("id", entity.Id),
                new Claim("strDOI", entity.StrDOI),
                new Claim("strCodeRol", entity.ObjRol.StrCode),
                new Claim("strDescriptionRol", entity.ObjRol.StrDescription),
                new Claim("strEmail", entity.StrEmail)
                
            };

            //Payload
            var payload = new JwtPayload
            (
               "https://localhost:5001/",
               "https://localhost:5001/",
               claims,
               DateTime.Now,
               expires: DateTime.Now.AddMinutes(5),
               DateTime.UtcNow.AddMinutes(5)
            );

            var token = new JwtSecurityToken(header, payload);
            string tkResponse = new JwtSecurityTokenHandler().WriteToken(token);

            response = new LoginResponseDTO
            {
                Codigo = entity.Id,
                Token = tkResponse
            };

            return response;
        }
    }
}
