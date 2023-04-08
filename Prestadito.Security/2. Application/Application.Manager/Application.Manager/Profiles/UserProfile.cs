using AutoMapper;
using Prestadito.Security.Application.Dto.User.CreateUser;
using Prestadito.Security.Application.Dto.User.DisableUser;
using Prestadito.Security.Application.Dto.User.GetUserById;
using Prestadito.Security.Application.Dto.User.GetUsersActive;
using Prestadito.Security.Application.Dto.User.UpdateUser;
using Prestadito.Security.Domain.MainModule.Entities;
using Prestadito.Security.Infrastructure.Data.Constants;
using Prestadito.Security.Infrastructure.Data.Utilities;

namespace Prestadito.Security.Application.Manager.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<CreateUserRequest, UserEntity>()
                .ForMember(dest => dest.StrPasswordHash, opt => opt.MapFrom(src => CryptoHelper.EncryptAES(src.StrPassword)))
                .ForMember(dest => dest.StrRolId, opt => opt.MapFrom(src => src.StrRolId))
                .ForMember(dest => dest.BlnActive, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.StrCreateUser, opt => opt.MapFrom(src => ConstantAPI.System.SYSTEM_USER));

            CreateMap<UserEntity, CreateUserResponse>()
                .ForMember(dest => dest.StrId, opt => opt.MapFrom(src => src.Id));

            CreateMap<UserEntity, GetUserByIdResponse>();

            CreateMap<UpdateUserRequest, UserEntity>()
                .ForMember(dest => dest.StrPasswordHash, opt => opt.MapFrom(src => CryptoHelper.EncryptAES(src.StrPassword)));

            CreateMap<UserEntity, UpdateUserResponse>()
                .ForMember(dest => dest.StrId, opt => opt.MapFrom(src => src.Id));

            CreateMap<UserEntity, DisableUserResponse>()
                .ForMember(dest => dest.StrId, opt => opt.MapFrom(src => src.Id));

            CreateMap<UserEntity, DeleteUserResponse>()
                .ForMember(dest => dest.StrId, opt => opt.MapFrom(src => src.Id));

            CreateMap<UserEntity, GetUsersActiveResponse>()
                .ForMember(dest => dest.StrId, opt => opt.MapFrom(src => src.Id));
        }
    }
}
