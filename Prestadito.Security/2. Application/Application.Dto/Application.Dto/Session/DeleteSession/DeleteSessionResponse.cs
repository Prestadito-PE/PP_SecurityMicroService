namespace Prestadito.Security.Application.Dto.User.GetUserById
{
    public class DeleteSessionResponse
    {
        public string StrId { get; set; } = null!;
        public string StrUserId { get; set; } = null!;
        public string StrIP { get; set; } = null!;
        public string StrDeviceName { get; set; } = null!;
        public int IntAttempts { get; set; } = 0;
        public string StrComment { get; set; } = null!;
        public string StrEnteredPasswordHash { get; set; } = null!;
    }
}
