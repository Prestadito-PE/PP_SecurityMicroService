using Microsoft.AspNetCore.Http;
using MongoDB.Driver;
using Prestadito.Security.Application.Dto.Login;
using Prestadito.Security.Application.Manager.Interfaces;
using Prestadito.Security.Application.Manager.QueryBuilder;
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

            var filterDefinitionUser = UserQueryBuilder.FindUserByEmail(request.StrEmail);
            var entityUser = await _userRepository.GetSingleAsync(filterDefinitionUser);
            if (entityUser is null)
            {
                responseModel = ResponseModel<LoginResponse>.GetResponse(ConstantMessages.Sessions.INCORRECT_CREDENTIALS);
                return Results.NotFound(responseModel);
            }

            if (!entityUser.BlnActive)
            {
                responseModel = ResponseModel<LoginResponse>.GetResponse(ConstantMessages.Users.USER_NOT_ACTIVE);
                return Results.UnprocessableEntity(responseModel);
            }

            if (entityUser.BlnLockByAttempts)
            {
                responseModel = ResponseModel<LoginResponse>.GetResponse(ConstantMessages.Sessions.USER_LOCKED_BY_MAX_ATTEMPS);
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
                    SettingProxyUtility.ParameterToValueInt(parameter.Item, ref maxAttempts);
                }

                var (filter, sort) = SessionQueryBuilder.FindSessionLastSortByDateLogin(entityUser.Id);
                var entitySession = await _sessionRepository.GetSingleFilterAndFindOptionsAsync(filter, sort);
                if (entitySession is not null)
                {
                    userLoginAttempts = entitySession.IntAttempts;
                }

                SessionEntity entitySessionError = new()
                {
                    StrUserId = entityUser.Id,
                    StrIP = clientIP,
                    StrDeviceName = request.StrDeviceName,
                    IntAttempts = userLoginAttempts + 1,
                    StrComment = (userLoginAttempts + 1 >= maxAttempts) ? ConstantMessages.Sessions.USER_LOCKED_BY_MAX_ATTEMPS : ConstantMessages.Sessions.INCORRECT_CREDENTIALS,
                    StrEnteredPasswordHash = passwordHash,
                    StrCreateUser = ConstantAPI.System.SYSTEM_USER
                };

                await _sessionRepository.InsertOneAsync(entitySessionError);

                if (string.IsNullOrEmpty(entitySessionError.Id))
                {
                    responseModel = ResponseModel<LoginResponse>.GetResponse(ConstantMessages.Sessions.SESSION_FAILED_TO_CREATE);
                    return Results.UnprocessableEntity(responseModel);
                }

                if (entitySessionError.IntAttempts < maxAttempts)
                {
                    responseModel = ResponseModel<LoginResponse>.GetResponse(ConstantMessages.Sessions.INCORRECT_CREDENTIALS);
                    return Results.UnprocessableEntity(responseModel);
                }

                var (filterDefinition, updateDefinition) = UserQueryBuilder.UpdateUserLockAttemps(entityUser.Id);
                var resultUpdate = await _userRepository.UpdateOneAsync(filterDefinition, updateDefinition);
                if (!resultUpdate)
                {
                    responseModel = ResponseModel<LoginResponse>.GetResponse(ConstantMessages.Users.USER_FAILED_TO_UPDATE);
                    return Results.UnprocessableEntity(responseModel);
                }
                responseModel = ResponseModel<LoginResponse>.GetResponse(ConstantMessages.Sessions.USER_LOCKED_BY_MAX_ATTEMPS);
                return Results.UnprocessableEntity(responseModel);
            }

            SessionEntity entity = new()
            {
                StrUserId = entityUser.Id,
                StrIP = clientIP,
                StrDeviceName = request.StrDeviceName,
                StrComment = ConstantMessages.Sessions.LOGIN_OK,
                StrEnteredPasswordHash = passwordHash,
                StrCreateUser = ConstantAPI.System.SYSTEM_USER
            };

            await _sessionRepository.InsertOneAsync(entity);
            if (string.IsNullOrEmpty(entity.Id))
            {
                responseModel = ResponseModel<LoginResponse>.GetResponse(ConstantMessages.Sessions.SESSION_FAILED_TO_CREATE);
                return Results.UnprocessableEntity(responseModel);
            }
            LoginResponse loginResponse = _jwtHelper.GenerateToken(entityUser);
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

            responseModel = ResponseModel<SessionEntity>.GetResponse(entity);
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
