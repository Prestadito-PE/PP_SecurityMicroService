namespace Prestadito.Security.Application.Dto.User
{
    public class UpdateUserDTO
    {
        public string Id { get; set; } = null!;
        public string StrDOI { get; set; } = null!;
        public string StrPassword { get; set; } = null!;
        public string StrRolCode { get; set; } = null!;
    }
}
