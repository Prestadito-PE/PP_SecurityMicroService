namespace Prestadito.Security.Infrastructure.Proxies.Settings.Models.Parameters
{
    public class ParameterModel
    {
        public string? Id { get; set; }
        public string StrCode { get; set; } = string.Empty;
        public string? StrParentCode { get; set; }
        public string StrName { get; set; } = string.Empty;
        public string StrValue { get; set; } = string.Empty;
        public string StrType { get; set; } = string.Empty;
        public string StrDescription { get; set; } = string.Empty;
        public bool BlnActive { get; set; }
        public string StrCreateUser { get; set; } = string.Empty;
        public DateTime DteCreatedAt { get; set; }
        public string? StrUpdateUSer { get; set; }
        public DateTime? DteUpdatedAt { get; set; }
    }
}
