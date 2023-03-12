namespace Prestadito.Security.Application.Manager.Models
{
    public class SessionModel
    {
        public string Id { get; set; } = string.Empty;
        public string StrUserId { get; set; } = string.Empty;
        public string StrIP { get; set; } = string.Empty;
        public string StrDeviceName { get; set; } = string.Empty;
        public int IntAttempts { get; set; }
        public string StrComment { get; set; } = string.Empty;
        public string StrEnteredPasswordHash { get; set; } = string.Empty;
        public DateTime DteLogin { get; set; }
    }
}
