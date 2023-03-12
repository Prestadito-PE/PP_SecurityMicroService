using MongoDB.Driver;
using Prestadito.Security.Application.Dto.Login;
using Prestadito.Security.Application.Manager.Interfaces;
using Prestadito.Security.Application.Manager.Models;
using Prestadito.Security.Application.Manager.Utilities;
using Prestadito.Security.Application.Services.Interfaces;
using Prestadito.Security.Domain.MainModule.Entities;
using Prestadito.Security.Infrastructure.Data.Constants;
using Prestadito.Security.Infrastructure.Data.Utilities;
using Prestadito.Security.Infrastructure.Proxies.Settings.DTO.Parameters;
using Prestadito.Security.Infrastructure.Proxies.Settings.Interfaces;
using System.Linq.Expressions;

namespace Prestadito.Security.API.Controller
{
    public class SessionsController : ISessionsController
    {
        private readonly ISessionRepository sessionRepository;
        private readonly IUserRepository userRepository;
        private readonly IJWTHelper jwtHelper;
        private readonly ISettingProxy settingProxy;

        public SessionsController(IDataService dataService, IJWTHelper _jwtHelper, ISettingProxy _settingProxy)
        {
            sessionRepository = dataService.Sessions;
            userRepository = dataService.Users;
            jwtHelper = _jwtHelper;
            settingProxy = _settingProxy;
        }

        public async ValueTask<IResult> DeleteSession(string id)
        {
            ResponseModel<SessionModel> responseModel;

            Expression<Func<SessionEntity, bool>> filter = f => f.Id == id;
            var entity = await sessionRepository.GetAsync(filter);
            if (entity is null)
            {
                responseModel = ResponseModel<SessionModel>.GetResponse("Session not exist");
                return Results.NotFound(responseModel);
            }

            var isSessionDeleted = await sessionRepository.DeleteOneAsync(filter);
            if (!isSessionDeleted)
            {
                responseModel = ResponseModel<SessionModel>.GetResponse("Session not deleted");
                return Results.UnprocessableEntity(responseModel);
            }

            var sessionModelItem = new SessionModel
            {
                Id = entity.Id,
                StrUserId = entity.StrUserId,
                StrIP = entity.StrIP,
                StrDeviceName = entity.StrDeviceName,
                IntAttempts = entity.IntAttempts,
                StrComment = entity.StrComment,
                StrEnteredPasswordHash = entity.StrEnteredPasswordHash,
                DteLogin = entity.DteLogin
            };
            responseModel = ResponseModel<SessionModel>.GetResponse(sessionModelItem);
            return Results.Json(responseModel);
        }

        public async ValueTask<IResult> GetAllSessions()
        {
            ResponseModel<SessionModel> responseModel;

            Expression<Func<SessionEntity, bool>> filter = f => true;
            var entities = await sessionRepository.GetAllAsync(filter);

            var sessionModelItems = entities.Select(u => new SessionModel
            {
                Id = u.Id,
                StrUserId = u.StrUserId,
                StrIP = u.StrIP,
                StrDeviceName = u.StrDeviceName,
                IntAttempts = u.IntAttempts,
                StrComment = u.StrComment,
                StrEnteredPasswordHash = u.StrEnteredPasswordHash,
                DteLogin = u.DteLogin
            }).ToList();

            responseModel = ResponseModel<SessionModel>.GetResponse(sessionModelItems);
            return Results.Json(responseModel);
        }

        public async ValueTask<IResult> Login(LoginDTO dto, HttpContext httpContext)
        {
            ResponseModel<LoginResponseDTO> responseModel;
            int maxAttempts = ConstantSettings.Settings.Parameter.LoginAttempts.LOGIN_MAX_ATTEMPTS;
            int userLoginAttempts = 0;
            var passwordHash = CryptoHelper.EncryptAES(dto.StrPassword);
            string clientIP = httpContext.Connection.RemoteIpAddress is null ? "" : httpContext.Connection.RemoteIpAddress.ToString();

            Expression<Func<UserEntity, bool>> filterUser = f => f.StrEmail == dto.StrEmail;
            var entityUser = await userRepository.GetAsync(filterUser);
            if (entityUser is null)
            {
                responseModel = ResponseModel<LoginResponseDTO>.GetResponse(ConstantMessages.Login.INCORRECT_CREDENTIALS);
                return Results.NotFound(responseModel);
            }

            if (!entityUser.BlnActive)
            {
                responseModel = ResponseModel<LoginResponseDTO>.GetResponse(ConstantMessages.Login.USER_NOT_ACTIVE);
                return Results.UnprocessableEntity(responseModel);
            }

            if (entityUser.StrStatusId == ConstantSettings.Parameter.UserStatus.STATUS_LOCK_ATTEMPTS)
            {
                responseModel = ResponseModel<LoginResponseDTO>.GetResponse(ConstantMessages.Login.USER_LOCKED_BY_MAX_ATTEMPS);
                return Results.UnprocessableEntity(responseModel);
            }

            if (entityUser.StrPasswordHash != passwordHash)
            {
                var parameter = await settingProxy.GetParameterByCode(
                    new GetParameterByCodeDTO
                    {
                        StrCode = ConstantAPI.MicroSetting.PARAMETER_MAX_ATTEMPS_CODE
                    });

                if (parameter is not null && !parameter.Error)
                {
                    if (int.TryParse(parameter.Item.StrValue, out int tempValue))
                    {
                        maxAttempts = tempValue;
                    }
                }

                #region CODE REFACTOR
                Expression<Func<SessionEntity, bool>> filterSessions = f => f.StrUserId == entityUser.Id;
                var sort = Builders<SessionEntity>.Sort.Descending(x => x.DteLogin);
                var findOptions = new FindOptions<SessionEntity, SessionEntity>()
                {
                    Limit = 1,
                    Sort = sort
                };
                var entitySession = await sessionRepository.GetSingleFindOptionsAsync(filterSessions, findOptions);
                #endregion
                if (entitySession is not null)
                {
                    userLoginAttempts = entitySession.IntAttempts;
                }

                if (userLoginAttempts + 1 >= maxAttempts)
                {
                    entityUser.StrStatusId = ConstantSettings.Parameter.UserStatus.STATUS_LOCK_ATTEMPTS;
                    await userRepository.ReplaceOneAsync(entityUser);
                }

                var entitySessionLoginError = new SessionEntity
                {
                    StrUserId = entityUser.Id,
                    StrIP = clientIP,
                    StrDeviceName = dto.StrDeviceName,
                    IntAttempts = userLoginAttempts + 1,
                    StrComment = ConstantMessages.Login.USER_LOCKED_BY_MAX_ATTEMPS,
                    StrEnteredPasswordHash = passwordHash,
                    StrCreateUser = ConstantAPI.System.SYSTEM_USER
                };
                _ = await sessionRepository.InsertOneAsync(entitySessionLoginError);

                if (entitySessionLoginError.IntAttempts >= maxAttempts)
                {
                    responseModel = ResponseModel<LoginResponseDTO>.GetResponse(ConstantMessages.Login.USER_LOCKED_BY_MAX_ATTEMPS);
                    return Results.UnprocessableEntity(responseModel);
                }

                responseModel = ResponseModel<LoginResponseDTO>.GetResponse(ConstantMessages.Login.INCORRECT_CREDENTIALS);
                return Results.UnprocessableEntity(responseModel);
            }

            UserModel userMap = new()
            {
                Id = entityUser.Id,
                StrDOI = entityUser.StrDOI,
                StrRolId = entityUser.StrRolId,
                BlnEmailValitated = entityUser.BlnEmailValitated,
                StrEmail = entityUser.StrEmail,
                StrStatusId = entityUser.StrStatusId
            };

            LoginResponseDTO loginResponseDTO = jwtHelper.GenerateToken(userMap);
            responseModel = ResponseModel<LoginResponseDTO>.GetResponse(loginResponseDTO);
            return Results.Json(responseModel);
        }
    }
}
