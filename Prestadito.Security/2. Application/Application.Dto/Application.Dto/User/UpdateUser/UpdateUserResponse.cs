namespace Prestadito.Security.Application.Dto.User.UpdateUser
{
    public class UpdateUserResponse
    {
        public string Id { get; set; } = string.Empty;
        public string StrEmail { get; set; } = string.Empty;
        public string StrRolId { get; set; } = string.Empty;
        public string StrDOI { get; set; } = string.Empty;
        public bool BlnEmailValitated { get; set; }
        public string StrStatusId { get; set; } = string.Empty;
    }
}
