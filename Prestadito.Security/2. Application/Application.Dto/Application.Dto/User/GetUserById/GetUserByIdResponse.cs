namespace Prestadito.Security.Application.Dto.User.GetUserById
{
    public class GetUserByIdResponse
    {
        public string StrId { get; set; } = string.Empty;
        public string StrEmail { get; set; } = string.Empty;
        public string StrRolId { get; set; } = string.Empty;
        public bool BlnEmailValidated { get; set; }
        public bool BlnLockByAttempts { get; set; }
        public bool BlnCompleteInformation { get; set; }
    }
}
