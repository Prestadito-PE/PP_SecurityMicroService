namespace Prestadito.Security.Application.Dto.User
{
    public class CreateUserDTO
    {
        public string StrDOI { get; set; } = null!;
        public string StrPassword { get; set; } = null!;
        public string StrRolCode { get; set; } = null!;
    }
}
