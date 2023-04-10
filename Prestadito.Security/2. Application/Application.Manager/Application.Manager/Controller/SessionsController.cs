using AutoMapper;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver;
using Prestadito.Security.Application.Dto.Session.Login;
using Prestadito.Security.Application.Dto.User.GetUserById;
using Prestadito.Security.Application.Manager.Interfaces;
using Prestadito.Security.Application.Manager.QueryBuilder;
using Prestadito.Security.Application.Manager.Utilities;
using Prestadito.Security.Domain.MainModule.Entities;
using Prestadito.Security.Infrastructure.Data.Constants;
using Prestadito.Security.Infrastructure.Data.Interface;
using Prestadito.Security.Infrastructure.Data.Utilities;
using Prestadito.Security.Infrastructure.Proxies.Settings.Interfaces;

namespace Prestadito.Security.Application.Manager.Controller
{
    public class SessionsController : ISessionsController
    {
        private readonly ISessionRepository _sessionRepository;
        private readonly IUserRepository _userRepository;
        private readonly IJWTHelper _jwtHelper;
        private readonly ISettingProxy _settingProxy;
        private readonly IMapper _mapper;


        public SessionsController(ISessionRepository sessionRepository, IUserRepository userRepository, IMapper mapper, IJWTHelper jwtHelper, ISettingProxy settingProxy)
        {
            _sessionRepository = sessionRepository;
            _userRepository = userRepository;
            _mapper = mapper;
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
                var parameter = await _settingProxy.GetParameterByCode(ConstantAPI.MicroSetting.PARAMETER_MAX_ATTEMPS_CODE);
                SettingProxyUtility.ParameterToValueInt(parameter, ref maxAttempts);

                var (filterDefinitionSession, findOptions) = SessionQueryBuilder.FindSessionLastSortByDateLogin(entityUser.Id);
                var entitySession = await _sessionRepository.GetSingleWithOptionsAsync(filterDefinitionSession, findOptions);
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

                var (_, updateDefinitionUser) = UserQueryBuilder.UpdateUserLockAttemps(entityUser.Id);
                var resultUpdate = await _userRepository.UpdateOneAsync(filterDefinitionUser, updateDefinitionUser);
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

        public async ValueTask<IResult> DeleteSession(DeleteSessionRequest request)
        {
            ResponseModel<DeleteSessionResponse> responseModel;

            var filterDefinition = SessionQueryBuilder.FindSessionById(request.StrId);
            var entity = await _sessionRepository.GetAsync(filterDefinition);
            if (entity is null)
            {
                responseModel = ResponseModel<DeleteSessionResponse>.GetResponse(ConstantMessages.Sessions.SESSION_NOT_FOUND);
                return Results.NotFound(responseModel);
            }

            var isSessionDeleted = await _sessionRepository.DeleteOneAsync(filterDefinition);
            if (!isSessionDeleted)
            {
                responseModel = ResponseModel<DeleteSessionResponse>.GetResponse(ConstantMessages.Sessions.SESSION_FAILED_TO_DELETE);
                return Results.UnprocessableEntity(responseModel);
            }

            var mapperResponse = _mapper.Map<DeleteSessionResponse>(entity);
            responseModel = ResponseModel<DeleteSessionResponse>.GetResponse(mapperResponse);
            return Results.Json(responseModel);
        }

        public async ValueTask<IResult> GetAllSessions()
        {
            ResponseModel<SessionEntity> responseModel;

            var filterDefinition = SessionQueryBuilder.FindAllSessions();
            var entities = await _sessionRepository.GetAsync(filterDefinition);

            responseModel = ResponseModel<SessionEntity>.GetResponse(entities.ToList());
            return Results.Json(responseModel);
        }
    }
}
