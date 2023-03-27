using Microsoft.AspNetCore.Http;
using MongoDB.Driver;
using Prestadito.Security.Application.Dto.Login;
using Prestadito.Security.Application.Manager.Interfaces;
using Prestadito.Security.Application.Manager.Utilities;
using Prestadito.Security.Domain.MainModule.Entities;
using Prestadito.Security.Infrastructure.Data.Constants;
using Prestadito.Security.Infrastructure.Data.Interface;
using Prestadito.Security.Infrastructure.Data.Utilities;
using Prestadito.Security.Infrastructure.Proxies.Settings.DTO.Parameters;
using Prestadito.Security.Infrastructure.Proxies.Settings.Interfaces;
using System.Linq.Expressions;

namespace Prestadito.Security.Application.Manager.Controller
{
    public class SessionsController : ISessionsController
    {
        private readonly ISessionRepository _sessionRepository;
        private readonly IUserRepository _userRepository;
        private readonly IJWTHelper _jwtHelper;
        private readonly ISettingProxy _settingProxy;

        public SessionsController(ISessionRepository sessionRepository, IUserRepository userRepository, IJWTHelper jwtHelper, ISettingProxy settingProxy)
        {
            _sessionRepository = sessionRepository;
            _userRepository = userRepository;
            _jwtHelper = jwtHelper;
            _settingProxy = settingProxy;
        }

        public async ValueTask<IResult> Login(LoginRequest request, HttpContext httpContext)
        {
            ResponseModel<LoginResponse> responseModel;
            int maxAttempts = ConstantSettings.Settings.Parameter.LoginAttempts.LOGIN_MAX_ATTEMPTS;
            int userLoginAttempts = 0;
            var passwordHash = CryptoHelper.EncryptAES(request.StrPassword);
            string clientIP = httpContext.Connection.RemoteIpAddress is null ? "" : httpContext.Connection.RemoteIpAddress.ToString();

            Expression<Func<UserEntity, bool>> filterUser = f => f.StrEmail == request.StrEmail;
            var entityUser = await _userRepository.GetSingleAsync(filterUser);
            if (entityUser is null)
            {
                responseModel = ResponseModel<LoginResponse>.GetResponse(ConstantMessages.Errors.Sessions.INCORRECT_CREDENTIALS);
                return Results.NotFound(responseModel);
            }

            if (!entityUser.BlnActive)
            {
                responseModel = ResponseModel<LoginResponse>.GetResponse(ConstantMessages.Errors.Sessions.USER_NOT_ACTIVE);
                return Results.UnprocessableEntity(responseModel);
            }

            if (entityUser.BlnLockByAttempts)
            {
                responseModel = ResponseModel<LoginResponse>.GetResponse(ConstantMessages.Errors.Sessions.USER_LOCKED_BY_MAX_ATTEMPS);
                return Results.UnprocessableEntity(responseModel);
            }

            if (entityUser.StrPasswordHash != passwordHash)
            {
                var parameter = await _settingProxy.GetParameterByCode(
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

                var entitySession = await _sessionRepository.GetSingleFindOptionsAsync(filterSessions, null);
                #endregion
                if (entitySession is not null)
                {
                    userLoginAttempts = entitySession.IntAttempts;
                }

                if (userLoginAttempts + 1 >= maxAttempts)
                {
                    entityUser.BlnLockByAttempts = true;
                    await _userRepository.UpdateOneAsync(filterUser, null);
                }

                var entitySessionLoginError = new SessionEntity
                {
                    StrUserId = entityUser.Id,
                    StrIP = clientIP,
                    StrDeviceName = request.StrDeviceName,
                    IntAttempts = userLoginAttempts + 1,
                    StrComment = ConstantMessages.Errors.Sessions.USER_LOCKED_BY_MAX_ATTEMPS,
                    StrEnteredPasswordHash = passwordHash,
                    StrCreateUser = ConstantAPI.System.SYSTEM_USER
                };
                await _sessionRepository.InsertOneAsync(entitySessionLoginError);

                if (entitySessionLoginError.IntAttempts >= maxAttempts)
                {
                    responseModel = ResponseModel<LoginResponse>.GetResponse(ConstantMessages.Errors.Sessions.USER_LOCKED_BY_MAX_ATTEMPS);
                    return Results.UnprocessableEntity(responseModel);
                }

                responseModel = ResponseModel<LoginResponse>.GetResponse(ConstantMessages.Errors.Sessions.INCORRECT_CREDENTIALS);
                return Results.UnprocessableEntity(responseModel);
            }

            UserEntity userMap = new()
            {
                Id = entityUser.Id,
                StrDOI = entityUser.StrDOI,
                StrRolId = entityUser.StrRolId,
                BlnEmailValidated = entityUser.BlnEmailValidated,
                StrEmail = entityUser.StrEmail,
                StrStatusId = entityUser.StrStatusId
            };

            LoginResponse loginResponse = _jwtHelper.GenerateToken(userMap);
            responseModel = ResponseModel<LoginResponse>.GetResponse(loginResponse);
            return Results.Json(responseModel);
        }

        public async ValueTask<IResult> DeleteSession(string id)
        {
            ResponseModel<SessionEntity> responseModel;

            Expression<Func<SessionEntity, bool>> filter = f => f.Id == id;
            var entity = await _sessionRepository.GetAsync(filter);
            if (entity is null)
            {
                responseModel = ResponseModel<SessionEntity>.GetResponse("Session not exist");
                return Results.NotFound(responseModel);
            }

            var isSessionDeleted = await _sessionRepository.DeleteOneAsync(filter);
            if (!isSessionDeleted)
            {
                responseModel = ResponseModel<SessionEntity>.GetResponse("Session not deleted");
                return Results.UnprocessableEntity(responseModel);
            }

            var SessionEntityItem = new SessionEntity
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
            responseModel = ResponseModel<SessionEntity>.GetResponse(SessionEntityItem);
            return Results.Json(responseModel);
        }

        public async ValueTask<IResult> GetAllSessions()
        {
            ResponseModel<SessionEntity> responseModel;

            Expression<Func<SessionEntity, bool>> filter = f => true;
            var entities = await _sessionRepository.GetAllAsync(filter);

            var SessionEntityItems = entities.Select(u => new SessionEntity
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

            responseModel = ResponseModel<SessionEntity>.GetResponse(SessionEntityItems);
            return Results.Json(responseModel);
        }
    }
}
