using Microsoft.IdentityModel.Tokens;
using Prestadito.Security.Application.Dto.Login;
using Prestadito.Security.Application.Manager.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Prestadito.Security.Application.Services.Utilities
{
    public class JWT
    {
        public static LoginResponseDTO GenerateToken(UserModel entity)
        {
            LoginResponseDTO response;
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
                StrId = entity.Id,
                StrToken = tkResponse
            };

            return response;
        }
    }
}
