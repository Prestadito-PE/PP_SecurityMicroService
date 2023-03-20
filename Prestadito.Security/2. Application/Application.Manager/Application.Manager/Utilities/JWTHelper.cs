using Microsoft.IdentityModel.Tokens;
using Prestadito.Security.Application.Dto.Login;
using Prestadito.Security.Domain.MainModule.Entities;
using Prestadito.Security.Infrastructure.Data.Interface;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Prestadito.Security.Infrastructure.Data.Utilities
{
    public class JWTHelper : IJWTHelper
    {
        private readonly IJWTSettings jwtSettings;

        public JWTHelper(IJWTSettings _jwtSettings)
        {
            jwtSettings = _jwtSettings;
        }

        public LoginResponse GenerateToken(UserEntity entity)
        {
            LoginResponse response;
            //Header 
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var header = new JwtHeader(signingCredentials);

            //add Claims
            var claims = new[]
            {
                new Claim("id", entity.Id),
                new Claim("strDOI", entity.StrDOI),
                new Claim("strRolId", entity.StrRolId),
                new Claim("strEmail", entity.StrEmail)
            };

            //Payload
            var payload = new JwtPayload
            (
               jwtSettings.Issuer,
               jwtSettings.Audience,
               claims,
               DateTime.Now,
               expires: DateTime.Now.AddMinutes(jwtSettings.ExpirationInMinutes),
               DateTime.UtcNow.AddMinutes(jwtSettings.ExpirationInMinutes)
            );

            var token = new JwtSecurityToken(header, payload);
            string tkResponse = new JwtSecurityTokenHandler().WriteToken(token);

            response = new LoginResponse
            {
                StrId = entity.Id,
                StrToken = tkResponse
            };

            return response;
        }
    }
}
