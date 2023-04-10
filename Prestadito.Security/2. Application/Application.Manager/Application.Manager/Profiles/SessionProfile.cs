using AutoMapper;
using Prestadito.Security.Application.Dto.User.GetUserById;
using Prestadito.Security.Domain.MainModule.Entities;

namespace Prestadito.Security.Application.Manager.Profiles
{
    public class SessionProfile : Profile
    {
        public SessionProfile()
        {
            CreateMap<SessionEntity, DeleteSessionResponse>()
                .ForMember(dest => dest.StrId, opt => opt.MapFrom(src => src.Id));
        }
    }
}
